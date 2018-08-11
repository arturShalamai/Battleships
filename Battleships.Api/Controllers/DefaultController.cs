using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Battleships.BLL;
using Battleships.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Api.Controllers
{
    [Produces("application/json")]
    [Route("")]
    public class DefaultController : Controller
    {
        private readonly IUnitOfWork _unit;

        public DefaultController(IUnitOfWork unit)
        {
            _unit = unit;
        }


        public class SigninPlatformModel
        {
            public string Id_token { get; set; }
            public string Access_token { get; set; }
            public string Token_type { get; set; }
            public string Expires_in { get; set; }
            public string Scope { get; set; }
            public string State { get; set; }
            public string session_state { get; set; }
        }

        [HttpPost]
        [Route("/signin-platform")]
        [Authorize(AuthenticationSchemes = "IdentityServer")]
        public async Task<IActionResult> SigninPlatform()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = Guid.Parse(claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var user = await _unit.PlayerRepo.SingleAsync(p => p.Id == userId);
            if (user != null) { return Ok(); }

            var player = new Player()
            {
                Id = userId,
                Email = GetUserClaim(ClaimTypes.NameIdentifier),
                FirstName = GetUserClaim("FirstName"),
                LastName = GetUserClaim("LastName"),
                NickName = GetUserClaim("NickName"),
                isExternal = true
            };

            await _unit.PlayerRepo.AddAsync(player);
            _unit.Save();

            return Ok();
        }

        private string GetUserClaim(string type) => User.Claims.FirstOrDefault(c => c.Type == type).Value;
    }
}