using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Hubs
{
    public class GameHub : Hub
    {
        //public async Task<string> StartGameOnClient(Guid gameId)
        //{
        //    return Clients.Groups. gameId;
        //}

        //public async Task ConnectToACahnnel(string channelId)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, channelId);
        //    await Clients.Caller.SendAsync("Start Game", "");
        //}
    }
}
