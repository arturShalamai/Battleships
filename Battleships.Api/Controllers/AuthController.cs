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
    public class AuthController : Controller
    {
        private readonly IPlayerService _playerSvc;
        private readonly ITokenService _tokenSvc;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthController(
            IPlayerService playerSvc,
            IMapper mapper,
            IConfiguration configuration,
            ITokenService tokenSvc)
        {
            _playerSvc = playerSvc;
            _mapper = mapper;
            _configuration = configuration;
            _tokenSvc = tokenSvc;
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
    }
}
