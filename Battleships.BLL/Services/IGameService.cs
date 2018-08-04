using Battleships.BLL.Models;
using Battleships.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleships.BLL.Services
{
    public interface IGameService
    {
        Task<Guid> StartGameAsync(Guid creatorId);
        Task<GameInfoModel> GetByIdAsync(Guid gameId, Guid userId);
        Task<List<Guid>> GetAllGames();
        Task JoinAsync(Guid gameId, string userId);
        Task PlaceShips(string ships, Guid userId, Guid gameId);
        Task<ShotResult> Shot(Guid gameId, Guid userId, int number);
        Task Surrender(Guid gameId, Guid userId);

        Task<bool> CheckAccess(Guid gameId, Guid userId);
    }
}