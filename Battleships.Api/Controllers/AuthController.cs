using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Battleships.BLL;
using Battleships.BLL.Services;
using Battleships.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    [Route("api/players")]
    public class AuthController : Controller
    {
        private readonly IPlayerService _playerSvc;
        private readonly IMapper _mapper;

        public AuthController(IPlayerService playerSvc, IMapper mapper)
        {
            _playerSvc = playerSvc;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]PlayerRegisterModel registerInfo)
        {
            if(!ModelState.IsValid) { return BadRequest("Invalid Model"); }
            if(!String.Equals(registerInfo.Password, registerInfo.ConfirmPassword)) { return BadRequest("Passwords doesn't match"); }
            var player = _mapper.Map<Player>(registerInfo);
            await _playerSvc.RegisterPlayer(player);
            return Ok();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task GetToken()
        //{

        //}
    }
}