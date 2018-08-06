using Battleships.BLL;
using Battleships.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Battleships.Api.Hubs
{
    [Authorize]
    [EnableCors("AllowAll")]
    public class GameHub : Hub<IGameHub>
    {
        private readonly IUnitOfWork _unit;

        public GameHub(IUnitOfWork unit) : base()
        {
            _unit = unit;
        }

        public async override Task OnConnectedAsync()
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));
            var player = await _unit.PlayerRepo.SingleAsync(p => p.Id == userId);

            await SubscribeToActiveGames(userId);

            try
            {
                await _unit.PlayerConnections.AddAsync(new PlayerConnection(player, Context.ConnectionId));
                await _unit.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Context.
            await base.OnConnectedAsync();
        }

        public async Task MakeTurn(int pos)
        {
            await Clients.All.MakeTurn(pos);
        }

        public async Task<string> GetConnId()
        {
            return Context.ConnectionId;
        }

        public async Task StartGame(string gameId)
        {
            await Clients.All.StartGame(gameId);
        }

        public async Task OponentReady(string groupName, string currPlayerId)
        {
            await Clients.GroupExcept(groupName, currPlayerId).OponentReady();
        }

        private string GetUserClaim(string type)
        {
            var claimsIdentity = (ClaimsIdentity)Context.User.Identity;
            return claimsIdentity.Claims.FirstOrDefault(c => c.Type == type).Value;
        }


        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Guid.Parse(GetUserClaim(ClaimTypes.NameIdentifier));

            try
            {
                var currConnectionEntity = await _unit.PlayerConnections.SingleAsync(p => p.ConnectionId == Context.ConnectionId);
                await _unit.PlayerConnections.DeleteOneAsync(currConnectionEntity);
                await _unit.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            await base.OnDisconnectedAsync(exception);
        }


        public string GetcurrentConn() => Context.ConnectionId;

        #region Helpers
        private async Task SubscribeToActiveGames(Guid userId)
        {
            var games = await _unit.GameRepo.WhereAsync(g => g.PlayersInfo.Any(p => p.PlayerId == userId) && g.Status == GameStatuses.Started, 
                                                        g => g.PlayersInfo);

            var gameIds = games.Select(g => g.Id).ToList();
            var distinct = gameIds.Distinct().ToList();

            foreach (var gameId in gameIds) { await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString()); }


            //await Clients.Group(gameIds[0].ToString()).MakeTurn(25);

        }

        #endregion


    }
}
