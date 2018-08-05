using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Battleships.Api.Hubs;
using Battleships.Api.Models;
using Battleships.BLL;
using Battleships.BLL.Services;
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
            var userId = GetUserClaim(ClaimTypes.NameIdentifier);
            var game = await _gamesSvc.GetByIdAsync(Guid.Parse(id), Guid.Parse(userId));

            return Ok(game);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> StartNewGame()
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            var gameId = await _gamesSvc.StartGameAsync(userId);

            await AddUserToGroup(userId, gameId.ToString());

            return Ok(gameId);
        }

        [HttpPost]
        [Route("join/{id}")]
        public async Task<IActionResult> JoinGame(string id)
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            await _gamesSvc.JoinAsync(Guid.Parse(id), userId.ToString());

            await AddUserToGroup(userId, id);

            var userConnections = await  GetUserHubsConnections(userId);
            await _gameHub.Clients.GroupExcept(id, userConnections).SendAsync("onPlayerJoined");

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
            var userId = GetUserClaim(ClaimTypes.NameIdentifier);
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

        private async Task AddUserToGroup(Guid userId, string groupName)
        {
            var userConnections = await GetUserHubsConnections(userId);
            foreach (var connection in userConnections) { await _gameHub.Groups.AddToGroupAsync(connection, groupName); }
        }

    }
}