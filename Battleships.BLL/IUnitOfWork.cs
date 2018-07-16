using Battleships.BLL.Repos;
using Battleships.DAL;
using System.Threading.Tasks;

namespace Battleships.BLL
{
    public interface IUnitOfWork
    {
        IRepository<GameInfo> GameInfo { get; }
        IRepository<Game> GameRepo { get; }
        IRepository<Player> PlayerRepo { get; }

        Task SaveAsync();

        void Dispose();
    }
}