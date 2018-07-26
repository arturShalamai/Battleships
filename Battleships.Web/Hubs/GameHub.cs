using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Web.Hubs
{
    [EnableCors("AllowAll")]
    public class GameHub : Hub<IGameHub>
    {
        public async Task StartGame(string gameId)
        {
            await Clients.All.StartGame(gameId);
        }
    }
}
