using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleships.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Game")]
    [Authorize]
    public class GameController : Controller
    {
        private readonly IGameService _gamesSvc;

        public GameController(IGameService gamesSvc)
        {
            _gamesSvc = gamesSvc;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> StartNewGame()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var gameId = await _gamesSvc.StartGameAsync(Guid.Parse(userId));

            return Ok(gameId);
        }
    }
}