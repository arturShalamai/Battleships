using Battleships.DAL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unit;

        public PlayerService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Player> GetPlayer(Expression<Func<Player, bool>> filter)
        {
            return await _unit.PlayerRepo.SingleAsync(filter);
        }

        public async Task AddPlayer(Player newPlayer)
        {
            await _unit.PlayerRepo.AddAsync(newPlayer);
            await _unit.SaveAsync();
        }

        public async Task Updatelayer(Player player)
        {
            await _unit.PlayerRepo.UpdateOneAsync(player);
            await _unit.SaveAsync();
        }

        public async Task BanPlayer(Guid playerId)
        {
            //player = _cont
            //await _unit.PlayerRepo.UpdateOneAsync(player);
        }

        public async Task Removelayer(Player player)
        {
            await _unit.PlayerRepo.DeleteOneAsync(player);
            _unit.SaveAsync();
        }
    }
}
