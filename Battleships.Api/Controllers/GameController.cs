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
            await _gameHub.Clients.Group(userId).SendAsync("getGame", game);
            return Ok(game);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> StartNewGame()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var gameId = await _gamesSvc.StartGameAsync(Guid.Parse(userId));

            //await _gameHub.Clients.Client(userEmail).SendAsync("oponentReady");
            //await _gameHub.Groups.AddToGroupAsync(userId, gameId.ToString());

            return Ok(gameId);
        }

        [HttpPost]
        [Route("join/{id}")]
        public async Task<IActionResult> JoinGame(string id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var playerConnection = await _unit.PlayerConnections.SingleAsync(p => p.PlayerId == Guid.Parse(userId));

            await _gamesSvc.JoinAsync(Guid.Parse(id), userId);
            await _gameHub.Groups.AddToGroupAsync(userId.ToString(), id);
            await _gameHub.Clients.GroupExcept(id, userId).SendAsync("oponentConnected");

            return Ok();
        }

        [HttpPost]
        [Route("placeships")]
        public async Task<IActionResult> PlaceShips([FromBody]PlaceShipsModel ships)
        {
            var userId = GetUserClaim(ClaimTypes.NameIdentifier);
            await _gamesSvc.PlaceShips(ships.Field, Guid.Parse(userId), ships.GameId);
            await _gameHub.Clients.GroupExcept(ships.GameId.ToString(), userId).SendAsync("oponentReady");

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
            return Ok(await _gamesSvc.CheckAccess(gameId, Guid.Parse(userId)));
        }

        private string GetUserClaim(string type) => User.Claims.FirstOrDefault(c => c.Type == type).Value;
    }
}