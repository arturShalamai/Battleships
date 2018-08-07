using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Battleships.Api.Hubs;
using Battleships.Api.Models;
using Battleships.BLL;
using Battleships.BLL.Services;
using Battleships.DAL;
using Battleships.Migrations.Migrations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Battleships.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Game")]
    [Authorize]
    public class GameController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IGameService _gamesSvc;
        private readonly IHubContext<GameHub> _gameHub;

        public GameController(
            IMapper mapper,
            IUnitOfWork unit,
            IGameService gamesSvc,
            IHubContext<GameHub> gameHub)
        {
            _unit = unit;
            _mapper = mapper;
            _gamesSvc = gamesSvc;
            _gameHub = gameHub;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            Guid gameId = Guid.NewGuid();
            if (!Guid.TryParse(id, out gameId)) { return Ok(); }
            var userId = GetUserClaim(ClaimTypes.NameIdentifier);
            var game = await _gamesSvc.GetByIdAsync(Guid.Parse(id), Guid.Parse(userId));

            return Ok(game);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> StartNewGame()
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            var game = await _gamesSvc.StartGameAsync(userId);

            var player = await _unit.PlayerRepo.SingleAsync(p => p.Id == userId);
            var playerConnections = await GetUserHubsConnections(userId);

            foreach (var connection in playerConnections)
            {
                var gameConn = new GamesConnection()
                {
                    Game = game,
                    Player = player,
                    ConnectionId = connection,
                    UserId = userId
                };

                await _unit.GameConnections.AddAsync(gameConn);
                _unit.Save();
            }

            await AddUserToGroup(game.Id, playerConnections);

            await _gameHub.Clients.Group(game.Id.ToString()).SendAsync("onGameCrated", "Signalr game created");

            return Ok(game.Id);
        }

        [HttpPost]
        [Route("join/{id}")]
        public async Task<IActionResult> JoinGame(Guid id)
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            var player = await _unit.PlayerRepo.SingleAsync(p => p.Id == userId);

            await _gamesSvc.JoinAsync(id, userId.ToString());

            var game = await _unit.GameRepo.SingleAsync(g => g.Id == id, g => g.PlayersInfo);
            var conns = await GetUserHubsConnections(game.PlayersInfo[0].PlayerId);

            foreach (var connection in conns)
            {
                var gameConn = new GamesConnection()
                {
                    Game = game,
                    Player = player,
                    ConnectionId = connection,
                    UserId = userId
                };

                await _unit.GameConnections.AddAsync(gameConn);
                _unit.Save();
            }


            var user = await _unit.PlayerRepo.SingleAsync(p => p.Id == userId);
            var userInfo = _mapper.Map<PlayerJoinedInfoModel>(user);
            await _gameHub.Clients.Clients(conns).SendAsync("onPlayerJoined", userInfo);

            //await AddConnectionToGameAsync(userId, id);
            //await AddUserToGroup(userId, id.ToString());

            //await _gameHub.Clients
            //    .GroupExcept(id.ToString(), userConnections)
            //    .SendAsync("onPlayerJoined");

            return Ok();
        }

        private async Task ReConnectGame(Guid userId, Guid gameId)
        {
            var conns = await GetUserHubsConnections(userId);
            var gameConns = await GetUserGameConnectionsAsync(userId, gameId);

            if (!conns.Any(pc => gameConns.Contains(pc)))
            {
                await _unit.GameConnections.DeleteManyAsync(gc => gc.UserId == userId && gc.GameId == gameId);
                _unit.Save();
                await AddConnectionToGameAsync(userId, gameId);
            }
        }

        [HttpPost]
        [Route("placeships")]
        public async Task<IActionResult> PlaceShips([FromBody]PlaceShipsModel ships)
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            await _gamesSvc.PlaceShips(ships.Field, userId, ships.GameId);

            var game = await _unit.GameRepo.SingleAsync(g => g.Id == ships.GameId, g => g.PlayersInfo);
            var secondUserId = game.PlayersInfo[0].PlayerId == userId ? game.PlayersInfo[1].PlayerId : game.PlayersInfo[0].PlayerId;

            var conns = await GetUserGameConnectionsAsync(secondUserId, ships.GameId);



            //await ReConnectGame(userId, ships.GameId);
            //var secondUserProxy = await GetSecondUserConnection(userId, ships.GameId);
            await _gameHub.Clients.Clients(conns).SendAsync("onPlayerReady");

            return Ok();
        }

        [HttpPost]
        [Route("{gameId}/fire/{number}")]
        public async Task<IActionResult> Shot(Guid gameId, int number)
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            var res = await _gamesSvc.Shot(gameId, userId, number);

            var game = await _unit.GameRepo.SingleAsync(g => g.Id == gameId, g => g.PlayersInfo);
            var secondUserId = game.PlayersInfo[0].PlayerId == userId ? userId : game.PlayersInfo[1].PlayerId;

            var conns = await GetUserGameConnectionsAsync(secondUserId, gameId);

            var shotResultModel = new GameShotResultModel()
            {
                GameId = gameId,
                Position = number,
                Result = res
            };

            await _gameHub.Clients.Clients(conns).SendAsync("onHit", shotResultModel);

            return Ok(new { Result = res.ToString() });
        }

        [HttpPost]
        [Route("{gameId}/surrender")]
        public async Task<IActionResult> Surrender(Guid gameId)
        {
            var userId = GetUserClaim(ClaimTypes.NameIdentifier);
            await _gamesSvc.Surrender(gameId, Guid.Parse(userId));

            return Ok();
        }

        [HttpGet]
        [Route("{gameId}/checkParticipant")]
        public async Task<IActionResult> CheckParticipant(Guid gameId)
        {
            Guid gameIdParse = Guid.NewGuid();
            var userId = GetUserClaim(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(gameId.ToString(), out gameIdParse) || gameId == Guid.Empty) { return Ok(false); }
            var connId = _unit.PlayerConnections.SingleAsync(pc => pc.PlayerId == Guid.Parse(userId));
            await _gameHub.Clients.Client(connId.ToString()).SendAsync("getGame", "Game Info");
            return Ok(await _gamesSvc.CheckAccess(gameId, Guid.Parse(userId)));
        }

        private string GetUserClaim(string type) => User.Claims.FirstOrDefault(c => c.Type == type).Value;


        private async Task<List<string>> GetUserHubsConnections(Guid userId)
        {
            var playerConnections = await _unit.PlayerConnections.Where(pc => pc.PlayerId == userId);
            return await playerConnections.Select(x => x.ConnectionId).ToListAsync();
        }

        private async Task<List<string>> GetUserGameConnectionsAsync(Guid userId, Guid gameId)
        {
            var connections = await _unit.GameConnections.Where(gc => gc.UserId == userId && gc.GameId == gameId);
            var gameconns = connections.ToList();

            var connectionsId = gameconns.Select(gc => gc.ConnectionId).ToList();

            return connectionsId;
        }

        private async Task AddUserToGroup(Guid userId, string groupName)
        {
            var playerConnections = await GetUserHubsConnections(userId);
            foreach (var connection in playerConnections) { await _gameHub.Groups.AddToGroupAsync(connection, groupName); }
        }

        private async Task AddUserToGroup(Guid groupName, List<string> userConnections)
        {
            foreach (var connection in userConnections) { await _gameHub.Groups.AddToGroupAsync(connection, groupName.ToString()); }
        }

        private async Task AddConnectionToGameAsync(Guid userId, Guid gameId)
        {
            var player = await _unit.PlayerRepo.SingleAsync(p => p.Id == userId);
            var game = await _unit.GameRepo.SingleAsync(g => g.Id == gameId);

            var playerConnections = await GetUserHubsConnections(userId);

            foreach (var connection in playerConnections)
            {
                var gameConn = new GamesConnection()
                {
                    Game = game,
                    Player = player,
                    ConnectionId = connection
                };

                await _unit.GameConnections.AddAsync(gameConn);
                _unit.Save();
            }
        }

        private async Task<List<string>> GetUsersGameConnections(Guid userId, Guid gameId)
        {
            var gameConns = await _unit.GameConnections.Where(gc => gc.UserId == userId && gc.GameId == gameId);
            return await gameConns.Select(g => g.ConnectionId).ToListAsync();
        }

        private async Task<IClientProxy> GetSecondUserConnection(Guid userId, Guid gameId)
        {
            var game = await _unit.GameRepo.SingleAsync(g => g.PlayersInfo.Any(p => p.PlayerId == userId), g => g.PlayersInfo);
            var secondUserId = game.PlayersInfo[0].PlayerId == userId ? game.PlayersInfo[1].PlayerId : game.PlayersInfo[0].PlayerId;

            var conns = await GetUserGameConnectionsAsync(secondUserId, gameId);
            return _gameHub.Clients.Clients(conns);
        }

    }
}