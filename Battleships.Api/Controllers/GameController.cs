using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private readonly IUnitOfWork _unit;
        private readonly IGameService _gamesSvc;
        private readonly IHubContext<GameHub> _gameHub;

        public GameController(
            IGameService gamesSvc,
            IHubContext<GameHub> gameHub,
            IUnitOfWork unit)
        {
            _gamesSvc = gamesSvc;
            _unit = unit;
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
                    ConnectionId = connection
                };

                await _unit.GameConnections.AddAsync(gameConn);
                await _unit.SaveAsync();
            }

            await AddUserToGroup(game.Id, playerConnections);

            return Ok(game.Id);
        }

        [HttpPost]
        [Route("join/{id}")]
        public async Task<IActionResult> JoinGame(Guid id)
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            try
            {
                await _gamesSvc.JoinAsync(id, userId.ToString());
            }
            catch (Exception ex) { return BadRequest(); }


            var userConnections = await GetUsersGameConnections(userId, id);

            //await AddConnectionToGameAsync(userId, id);
            await AddUserToGroup(userId, userConnections);

            await _gameHub.Clients.GroupExcept(id.ToString(), userConnections).SendAsync("onPlayerJoined");

            return Ok();
        }

        [HttpPost]
        [Route("placeships")]
        public async Task<IActionResult> PlaceShips([FromBody]PlaceShipsModel ships)
        {
            var userId = GetUserClaim(ClaimTypes.NameIdentifier);
            var connId = await _unit.PlayerConnections.SingleAsync(pc => pc.PlayerId == Guid.Parse(userId));
            await _gameHub.Clients.Client(connId.ConnectionId.ToString()).SendAsync("getGame", "Game Info");
            await _gamesSvc.PlaceShips(ships.Field, Guid.Parse(userId), ships.GameId);
            //await _gameHub.Clients.GroupExcept(ships.GameId.ToString(), userId).SendAsync("oponentReady");

            //User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            return Ok();
        }

        [HttpPost]
        [Route("{gameId}/fire/{number}")]
        public async Task<IActionResult> Shot(Guid gameId, int number)
        {
            var userId = GetUserClaim(ClaimTypes.NameIdentifier);
            var res = await _gamesSvc.Shot(gameId, Guid.Parse(userId), number);

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
            if(!Guid.TryParse(gameId.ToString(), out gameIdParse)) { return Ok(false); }
            var connId = _unit.PlayerConnections.SingleAsync(pc => pc.PlayerId == Guid.Parse(userId));
            await _gameHub.Clients.Client(connId.ToString()).SendAsync("getGame", "Game Info");
            return Ok(await _gamesSvc.CheckAccess(gameId, Guid.Parse(userId)));
        }

        private string GetUserClaim(string type) => User.Claims.FirstOrDefault(c => c.Type == type).Value;


        private async Task<List<string>> GetUserHubsConnections(Guid userId)
        {
            var playerConnections = await _unit.PlayerConnections.WhereAsync(pc => pc.PlayerId == userId);
            return await playerConnections.Select(x => x.ConnectionId).ToListAsync();
        }

        private async Task<List<string>> GetUserGameConnectionsAsync(Guid userId, Guid gameId)
        {
            var connections = await _unit.GameConnections.WhereAsync(gc => gc.UserId == userId && gc.GameId == gameId);
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
                    ConnectionId = connection
                };

                await _unit.GameConnections.AddAsync(gameConn);
                await _unit.SaveAsync();
            }
        }

        private async Task<List<string>> GetUsersGameConnections(Guid userId, Guid gameId)
        {
            var gameConns = await _unit.GameConnections.WhereAsync(gc => gc.UserId == userId && gc.GameId == gameId);
            return await gameConns.Select(g => g.ConnectionId).ToListAsync();
        }

    }
}