using Battleships.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleships.BLL.Services
{
    public interface IGameService
    {
        Task<Guid> StartGameAsync(Guid creatorId);
        Task<Game> GetByIdAsync(Guid gameId);
        Task<List<Guid>> GetAllGames();
    }
}