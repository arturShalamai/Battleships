using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleships.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Battleships.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Game")]
    [EnableCors("AllowAll")]
    public class GameController : Controller
    {
        private readonly IHubContext<GameHub, IGameHub> _gameHub;

        public GameController(IHubContext<GameHub, IGameHub> gameHub)
        {
            _gameHub = gameHub;
        }

        [HttpPost]
        [Authorize]
        [Route("{id}")]
        public async Task StartGame(string id)
        {
            await _gameHub.Clients.All.StartGame(id);
        }
    }
}