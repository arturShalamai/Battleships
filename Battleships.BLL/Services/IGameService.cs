using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleships.BLL.Services
{
    public interface IGameService
    {
        Task<Guid> StartGameAsync();
        Task<List<Guid>> GetAllGames();
    }
}