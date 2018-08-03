using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Battleships.Api.Hubs
{
    [Authorize]
    [EnableCors("AllowAll")]
    public class GameHub : Hub<IGameHub>
    {
        public async Task MakeTurn(int pos)
        {
            await Clients.All.MakeTurn(pos);
        }

        public async Task StartGame(string gameId)
        {
            await Clients.All.StartGame(gameId);
        }

        public async Task OponentReady(string groupName, string currPlayerId) {
            await Clients.GroupExcept(groupName, currPlayerId).OponentReady();
        }
    }
}
