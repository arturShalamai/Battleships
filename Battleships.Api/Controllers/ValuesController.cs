using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

using Battleships.Api.Hubs;
using Battleships.BLL;
using Battleships.BLL.Services;
using Battleships.DAL;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Battleships.Api.Controllers
{
    [Authorize]
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IPlayerService _playerSvc;
        private readonly IGameService _gameSvc;
        private readonly IHubContext<GameHub> _gameHub;
        private readonly IUnitOfWork _unit;

        public ValuesController(
            IGameService gameSvc, 
            IPlayerService playerSvc, 
            IUnitOfWork unit,
            IHubContext<GameHub> gameHub)
        {
            _unit = unit;
            _gameHub = gameHub;
            _gameSvc = gameSvc;
            _playerSvc = playerSvc;
        }

        // GET api/values
        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var userId = GetUserClaim(ClaimTypes.NameIdentifier);
            await _gameHub.Clients.Client(userId).SendAsync("getGame", "Game Info");
            return Ok(new string[] { "value1", "value2" });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            var userConnections = await GetUserHubsConnections(userId);

            //var secondUserId = _unit.GameRepo.SingleAsync(g => g.Ga)

            //await AddConnectionToGameAsync(userId, id);
            //await AddUserToGroup(userId, userConnections);

            foreach (var connection in userConnections)
            {
                await _gameHub.Groups.AddToGroupAsync(connection, id.ToString());
            }

            await _gameHub.Clients.Group(id.ToString()).SendAsync("onPlayerJoined");

            //var disco = await DiscoveryClient.GetAsync("https://localhost:44362");

            //var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("Platform.ProfileService");

            //var client = new HttpClient();
            //client.SetBearerToken(tokenResponse.AccessToken);

            //var resp = client.GetStringAsync("https://");

            return Ok("value");
        }



        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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

                if(_unit.GameConnections.SingleAsync(gc => gc.UserId == userId && 
                                                           gc.GameId ==  gameId && 
                                                           gc.ConnectionId == gameConn.ConnectionId) == null)
                {
                    await _unit.GameConnections.AddAsync(gameConn);
                    await _unit.SaveAsync();
                }
            }
        }

        private async Task<List<string>> GetUsersGameConnections(Guid userId, Guid gameId)
        {
            var gameConns = await _unit.GameConnections.WhereAsync(gc => gc.UserId == userId && gc.GameId == gameId);
            return await gameConns.Select(g => g.ConnectionId).ToListAsync();
        }
    }
}
