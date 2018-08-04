using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Battleships.Api.Services;
using Battleships.BLL;
using Battleships.BLL.Models;
using Battleships.BLL.Services;
using Battleships.DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Battleships.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    [Route("api/players")]
    [Authorize]
    public class AuthController : Controller
    {
        private readonly IPlayerService _playerSvc;
        private readonly ITokenService _tokenSvc;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unit;

        public AuthController(
            IPlayerService playerSvc,
            IMapper mapper,
            IConfiguration configuration,
            ITokenService tokenSvc,
            IUnitOfWork unit)
        {
            _playerSvc = playerSvc;
            _mapper = mapper;
            _configuration = configuration;
            _tokenSvc = tokenSvc;
            _unit = unit;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]PlayerRegisterModel registerInfo)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Model"); }
            if (!String.Equals(registerInfo.Password, registerInfo.ConfirmPassword)) { return BadRequest("Passwords doesn't match"); }
            var player = _mapper.Map<Player>(registerInfo);
            await _playerSvc.RegisterPlayer(player);
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("token")]
        public async Task<IActionResult> GetToken(PlayerLoginModel loginModel)
        {
            if (!ModelState.IsValid) { return BadRequest(); }
            var verifyRes = await _playerSvc.ValidateCredentials(loginModel.Email, loginModel.Password);
            if (verifyRes == PasswordVerificationResult.Success)
            {
                var token = await _tokenSvc.GetTokenAsync(loginModel.Email);
                return Ok(new { token });
            }

            return BadRequest("Invalid credentials");
        }

        [HttpPost]
        [Route("validateToken")]
        public IActionResult ValidateToken()
        {
            return Ok();
        }

        [HttpPost]
        [Route("signin-platform")]
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
            await _unit.SaveAsync();

            return Ok();
        }

        private string GetUserClaim(string type)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            return claimsIdentity.Claims.FirstOrDefault(c => c.Type == type).Value;
        }

    }

}
