using System.Threading.Tasks;

namespace Battleships.BLL.Services
{
    public interface IGameService
    {
        Task<int> StartGameAsync();
    }
}