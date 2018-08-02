using Battleships.BLL.Models;
using Battleships.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unit;

        public GameService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<List<Guid>> GetAllGames()
        {
            var games = await _unit.GameRepo.AllAsync();
            return games.Select(g => g.Id).ToList();
        }

        public async Task<Game> GetByIdAsync(Guid gameId)
        {
            return await _unit.GameRepo.SingleAsync(g => g.Id == gameId);
        }

        public async Task<Guid> StartGameAsync(Guid creatorId)
        {
            var creator = await _unit.PlayerRepo.SingleAsync(p => p.Id == creatorId);
            var game = new DAL.Game(creator);
            await _unit.GameRepo.AddAsync(game);

            await _unit.SaveAsync();

            return game.Id;
        }
    }
}
