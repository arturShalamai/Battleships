using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleships.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Controllers
{
    [Produces("application/json")]
    [Route("api/Game")]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> StartNewGame()
        {
            var gameId = await _gameService.StartGameAsync();

            return Ok(new { GameId = gameId });

        }
    }
}