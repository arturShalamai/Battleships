using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Battleships.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlayerService _playerSvc;

        public HomeController(IPlayerService playerSvc)
        {
            _playerSvc = playerSvc;

            //_playerSvc.AddPlayer(new DAL.Player()
            //{
            //    FirstName = "Player1",
            //    LastName = "Fake Player1",
            //    NickName = "Player1",
            //    DoB = DateTime.Now,
            //});

            //_playerSvc.AddPlayer(new DAL.Player()
            //{
            //    FirstName = "Player2",
            //    LastName = "Fake Player2",
            //    NickName = "Player2",
            //    DoB = DateTime.Now,
            //});
        }

        public async Task<IActionResult> Index()
        {
            await _playerSvc.AddPlayer(new DAL.Player()
            {
                FirstName = "Player8",
                LastName = "Fake Player8",
                NickName = "Player8",
                DoB = DateTime.Now,
            });

            await _playerSvc.AddPlayer(new DAL.Player()
            {
                FirstName = "Player9",
                LastName = "Fake Player9",
                NickName = "Player9",
                DoB = DateTime.Now,
            });

            return  View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
