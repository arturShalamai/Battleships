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

        public async Task JoinAsync(Guid gameId, string userId)
        {
            userId = userId.ToUpper();

            var gameInfo = await _unit.GameRepo.SingleAsync(g => g.Id == gameId, g => g.GameInfo);
            var currPlayer = await _unit.PlayerRepo.SingleAsync(p => p.Id.ToString() == userId);
            gameInfo.AddPlayer(currPlayer);

            await _unit.GameRepo.UpdateOneAsync(gameInfo);
            _unit.SaveAsync();
        }

        public async Task<Guid> StartGameAsync(Guid creatorId)
        {
            var creator = await _unit.PlayerRepo.SingleAsync(p => p.Id == creatorId);
            var game = new Game(creator);
            await _unit.GameRepo.AddAsync(game);

            _unit.SaveAsync();

            return game.Id;
        }

        public async Task Fire(int positions, string userId, Guid gameId)
        {
            //userId = userId.ToUpper();
            //var gameInfo = await _unit.GameRepo.SingleAsync(g => g.Id == gameId, g => g.PlayersInfo, 
            //                                                                     g => g.GameInfo);

            //var currPlayer = await _unit.PlayerRepo.SingleAsync(p => p.Id.ToString() == userId);

            //if(gameInfo.PlayersInfo[0].PlayerId == Guid.Parse(userId))
            //{
            //    gameInfo.
            //}

        }
    }
}
