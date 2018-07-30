using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleships.BLL;
using Battleships.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        //private readonly IPlayerService _playerSvc;

        public AuthController()
        {

        }

        //public AuthController(IPlayerService playerSvc)
        //{
        //    //_playerSvc = playerSvc;
        //}

        [HttpPost]
        [AllowAnonymous]
        public async Task Register([FromBody]PlayerRegisterModel registerInfo)
        {
            //var player = _mapper.Map<Player>(registerInfo);
            //await _playerSvc.RegisterPlayer();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task GetToken()
        {

        }
    }
}