using Battleships.DAL;
using Microsoft.AspNetCore.Identity;
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
        private readonly IPasswordHasher<Player> _hasher;

        public PlayerService(IUnitOfWork unit)
        {
            _unit = unit;
            _hasher = new PasswordHasher<Player>();
        }

        public async Task<Player> GetPlayer(Expression<Func<Player, bool>> filter)
        {
            return await _unit.PlayerRepo.SingleAsync(filter);
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

        public async Task RegisterPlayer(Player newPlayer)
        {

            var checkUser = await _unit.PlayerRepo.WhereAsync(p => String.Equals(p.Email, newPlayer.Email, StringComparison.OrdinalIgnoreCase) ||
                                                             String.Equals(p.NickName, newPlayer.NickName, StringComparison.OrdinalIgnoreCase));

            if (checkUser.Count != 0) { throw new Exception("There already user with such email or login"); }

            var hashedPassword = _hasher.HashPassword(newPlayer, newPlayer.Password);
            newPlayer.Password = hashedPassword;

            await _unit.PlayerRepo.AddAsync(newPlayer);
            await _unit.SaveAsync();
        }
    }
}
