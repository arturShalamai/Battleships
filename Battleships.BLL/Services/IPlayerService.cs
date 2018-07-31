using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Battleships.DAL;
using Microsoft.AspNetCore.Identity;

namespace Battleships.BLL.Services
{
    public interface IPlayerService
    {
        Task<PasswordVerificationResult> ValidateCredentials(string email, string password);
        Task RegisterPlayer(Player newPlayer);
        Task BanPlayer(Guid playerId);
        Task<Player> GetPlayer(Expression<Func<Player, bool>> filter);
        Task Removelayer(Player player);
        Task Updatelayer(Player player);
    }
}