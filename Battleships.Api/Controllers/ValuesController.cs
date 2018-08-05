using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

using Battleships.Api.Hubs;
using Battleships.BLL;
using Battleships.BLL.Services;

using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
            IUnitOfWork unit)
        {
            _unit = unit;
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
        public string  Get(int id)
        {
            //var disco = await DiscoveryClient.GetAsync("https://localhost:44362");

            //var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("Platform.ProfileService");

            //var client = new HttpClient();
            //client.SetBearerToken(tokenResponse.AccessToken);

            //var resp = client.GetStringAsync("https://");

            return "value";
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
    }
}
