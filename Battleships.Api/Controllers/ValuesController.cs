using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Battleships.BLL.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [Authorize]
    public class ValuesController : Controller
    {
        private readonly IPlayerService _playerSvc;
        private readonly IGameService _gameSvc;

        public ValuesController(IGameService gameSvc, IPlayerService playerSvc)
        {
            _gameSvc = gameSvc;
            _playerSvc = playerSvc;
        }

        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            return Ok(new string[] { "value1", "value2" });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string  Get(int id)
        {
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
    }
}
