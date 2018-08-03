using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Battleships.Api.Hubs;
using Battleships.Api.Models;
using Battleships.BLL.Services;
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
        private readonly IGameService _gamesSvc;
        private readonly IHubContext<GameHub> _gameHub;

        public GameController(IGameService gamesSvc, IHubContext<GameHub> gameHub)
        {
            _gamesSvc = gamesSvc;
            _gameHub = gameHub;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var res = await _gamesSvc.GetByIdAsync(Guid.Parse(id));
            return Ok(new { Id = res.Id });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> StartNewGame()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var gameId = await _gamesSvc.StartGameAsync(Guid.Parse(userId));

            //await _gameHub.Clients.Client(userEmail).SendAsync("oponentReady");
            //await _gameHub.Groups.AddToGroupAsync(userId, gameId.ToString());

            return RedirectToAction(nameof(GetById), new { id = gameId });
        }

        [HttpPost]
        [Route("join/{id}")]
        public async Task<IActionResult> JoinGame(string id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            await _gamesSvc.JoinAsync(Guid.Parse(id), userId);

            return Ok();
        }

        [HttpPost]
        [Route("placeships")]
        public async Task<IActionResult> PlaceShips([FromBody]PlaceShipsModel ships)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var shipsMapped = String.Join(String.Empty, ships.Field);
            await _gamesSvc.PlaceShips(shipsMapped, Guid.Parse(userId), ships.GameId);
            await _gameHub.Clients.GroupExcept(ships.GameId.ToString(), userId).SendAsync("oponentReady");

            return Ok();
        }

        [HttpPost]
        [Route("{gameId}/fire/{number}")]
        public async Task<IActionResult> Shot(Guid gameId, int number)
        {
            var userId = GetUserClaim(ClaimTypes.NameIdentifier);
            var res = await _gamesSvc.Shot(gameId, Guid.Parse(userId), number);

            return Ok();
        }

        private string GetUserClaim(string type) => User.Claims.FirstOrDefault(c => c.Type == type).Value;

    }
}