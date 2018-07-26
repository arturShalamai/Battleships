using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
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
    }
}
