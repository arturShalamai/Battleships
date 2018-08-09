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
        private readonly IScoreService _scoreService;

        public GameController(
            IMapper mapper,
            IUnitOfWork unit,
            IGameService gamesSvc,
            IScoreService scoreService,
            IHubContext<GameHub> gameHub)
        {
            _unit = unit;
            _mapper = mapper;
            _gamesSvc = gamesSvc;
            _gameHub = gameHub;
            _scoreService = scoreService;
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

            await AddConnectionToGameAsync(userId, game.Id);
            await AddUserToGroup(userId, game.Id.ToString());

            await _gameHub.Clients.Group(game.Id.ToString()).SendAsync("onGameCrated", game.Id);

            return Ok(game.Id);
        }

        [HttpPost]
        [Route("join/{id}")]
        public async Task<IActionResult> JoinGame(Guid id)
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));

            await _gamesSvc.JoinAsync(id, userId.ToString());

            await AddConnectionToGameAsync(userId, id);

            var secondDuserProxy = await GetSecondUserConnection(userId, id);
            var player = await _unit.PlayerRepo.SingleAsync(p => p.Id == userId);
            await secondDuserProxy.SendAsync("onPlayerJoined", _mapper.Map<PlayerJoinedInfoModel>(player));

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

            var secondUserProxy = await GetSecondUserConnection(userId, ships.GameId);
            await secondUserProxy.SendAsync("onPlayerReady");

            return Ok();
        }

        [HttpPost]
        [Route("{gameId}/fire/{number}")]
        public async Task<IActionResult> Shot(Guid gameId, int number)
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            var res = await _gamesSvc.Shot(gameId, userId, number);

            if (res == ShotResult.Win)
            {

                var player = await _unit.PlayerRepo.SingleAsync(p => p.Id == userId);
                player.Score += 10;
                await _unit.PlayerRepo.UpdateOneAsync(player);
                _unit.Save();

                await _scoreService.SendScores(player.Id, player.Score);

                await _gameHub.Clients.Group(gameId.ToString()).SendAsync("onGameEnd", new
                {
                    GameId = gameId,
                    Winner = userId
                });

                await StopGameConnections(gameId);

                return Ok(new { Result = res.ToString() });
            }

            var secondUserProxy = await GetSecondUserConnection(userId, gameId);

            var shotResultModel = new GameShotResultModel()
            {
                GameId = gameId,
                Position = number,
                Result = res.ToString()
            };

            await secondUserProxy.SendAsync("onHit", shotResultModel);

            return Ok(new { Result = res.ToString() });
        }

        [HttpPost]
        [Route("{gameId}/surrender")]
        public async Task<IActionResult> Surrender(Guid gameId)
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            await _gamesSvc.Surrender(gameId, userId);

            var game = await _unit.GameRepo.SingleAsync(g => g.Id == gameId, g => g.PlayersInfo);
            var winnerId = game.PlayersInfo[0].PlayerId == userId ? game.PlayersInfo[1].PlayerId : game.PlayersInfo[0].PlayerId;

            var player = await _unit.PlayerRepo.SingleAsync(p => p.Id == winnerId);
            player.Score += 10;
            await _unit.PlayerRepo.UpdateOneAsync(player);
            _unit.Save();

            //await _scoreService.SendScores(player.Id, player.Score);

            await _gameHub.Clients.Group(gameId.ToString()).SendAsync("onGameEnd", new
            {
                GameId = gameId,
                Winner = winnerId
            });

            await StopGameConnections(gameId);

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
            return connections.Select(gc => gc.ConnectionId).ToList();
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
                    ConnectionId = connection,
                    UserId = player.Id
                };

                await _unit.GameConnections.AddAsync(gameConn);

                _unit.Save();
            }

            await AddUserToGroup(gameId, playerConnections);
        }

        private async Task StopGameConnections(Guid gameId) =>
            await _unit.GameConnections.DeleteManyAsync(g => g.GameId == gameId);

        private async Task<List<string>> GetUsersGameConnections(Guid userId, Guid gameId)
        {
            var gameConns = await _unit.GameConnections.Where(gc => gc.UserId == userId && gc.GameId == gameId);
            return await gameConns.Select(g => g.ConnectionId).ToListAsync();
        }

        private async Task<IClientProxy> GetSecondUserConnection(Guid userId, Guid gameId)
        {
            var game = await _unit.GameRepo.SingleAsync(g => g.Id == gameId, g => g.PlayersInfo);
            var secondUserId = game.PlayersInfo[0].PlayerId == userId ? game.PlayersInfo[1].PlayerId : game.PlayersInfo[0].PlayerId;

            var conns = await GetUserGameConnectionsAsync(secondUserId, gameId);
            return _gameHub.Clients.Clients(conns);
        }

    }
}