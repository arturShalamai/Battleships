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
            var gameInfo = await _unit.GameRepo.SingleAsync(g => g.Id == gameId, g => g.PlayersInfo);
            var currPlayer = await _unit.PlayerRepo.SingleAsync(p => p.Id.ToString() == userId);
            gameInfo.AddPlayer(currPlayer);

            await _unit.GameRepo.UpdateOneAsync(gameInfo);
            await _unit.SaveAsync();
        }

        public async Task<Guid> StartGameAsync(Guid creatorId)
        {
            var creator = await _unit.PlayerRepo.SingleAsync(p => p.Id == creatorId, c => c.GamesInfo);
            var game = new DAL.Game(creator);
            await _unit.GameRepo.AddAsync(game);

            await _unit.SaveAsync();

            return game.Id;
        }

        public async Task PlaceShips(string ships, Guid userId, Guid gameId)
        {
            var game = await _unit.GameRepo.SingleAsync(g => g.Id == gameId, g => g.GameInfo,
                                                                       g => g.PlayersInfo);

            ValidateShipsPlacement(ships, game, userId);

            if (game.PlayersInfo[0].PlayerId == userId)
            {
                game.GameInfo.FirstUserField = ships;
                game.GameInfo.FirstUserReady = true;
            }
            else
            {
                game.GameInfo.SecondUserField = ships;
                game.GameInfo.SecondUserReady = true;
            }

            await _unit.GameRepo.UpdateOneAsync(game);

            await _unit.SaveAsync();

        }

        private void ValidateShipsPlacement(string ships, Game game, Guid userId)
        {
            if (game == null || !game.PlayersInfo.Any(p => p.PlayerId == userId)) { throw new Exception("Wrong game"); }
            if (game.Status != GameStatuses.Waiting) { throw new Exception("Already placed"); }
            if (ships.Count() != 42 || ships.Count(c => c == 'x') != 16) { throw new Exception("Wrong field"); }
            if (game.PlayersInfo[0].PlayerId == userId && game.GameInfo.FirstUserReady) { throw new Exception("Already placed"); }
            if (game.PlayersInfo[1].PlayerId == userId && game.GameInfo.SecondUserReady) { throw new Exception("Already placed"); }
        }

    }
}
