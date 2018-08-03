using Battleships.BLL.Models;
using Battleships.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.BLL.Services
{
    public enum ShotResult { Hit, Miss, Win }

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

        public async Task<ShotResult> Shot(Guid gameId, Guid userId, int number)
        {
            var game = await _unit.GameRepo.SingleAsync(g => g.Id == gameId, g => g.PlayersInfo, g => g.GameInfo);
            var field = "";

            if (HasAccess(game, userId) && CheckTurn(game, userId))
            {
                field = game.GameInfo.Turn ? game.GameInfo.SecondUserField : game.GameInfo.FirstUserField;
            }
            else { throw new Exception("Wrong game"); }

            if (IsValidShot(field, number))
            {
                var sb = new StringBuilder(field);
                var res = new ShotResult();

                if (field[number] == '█')
                {
                    sb[number-1] = 'x';
                    res = ShotResult.Hit;
                    if (!field.Any(x => x == '█'))
                    {
                        SetWinner(game, userId);
                        res = ShotResult.Win;
                    }
                }
                else
                {
                    sb[number] = '·';
                    res = ShotResult.Miss;
                }

                game.SwitchTurn();

                if (game.PlayersInfo[0].PlayerId == userId) { game.GameInfo.FirstUserField = sb.ToString(); }
                else { game.GameInfo.SecondUserField = sb.ToString(); }

                await _unit.GameRepo.UpdateOneAsync(game);
                await _unit.SaveAsync();

                return res;
            }
            else { throw new Exception("Invalid shot"); }
        }

        private void SetWinner(Game game, Guid userId)
        {
            game.Status = GameStatuses.Finished;
            game.Winner = game.PlayersInfo[0].PlayerId == userId ? false : true;
        }

        #region Helpers
        private bool HasAccess(Game game, Guid userId)
        {
            return game.PlayersInfo.Any(p => p.PlayerId == userId);
        }

        private bool CheckTurn(Game game, Guid userId)
        {
            if (game.PlayersInfo[0].PlayerId == userId && !game.GameInfo.Turn ||
                game.PlayersInfo[1].PlayerId == userId && game.GameInfo.Turn) { return true; }
            return false;
        }

        private bool IsValidShot(string field, int number)
        {
            var fieldItem = field[number];
            return !(number < 0 || number >= 42 || field[number] == 'x' || field[number] == '·');
        }

        private void ValidateShipsPlacement(string ships, Game game, Guid userId)
        {
            if (game == null || !game.PlayersInfo.Any(p => p.PlayerId == userId)) { throw new Exception("Wrong game"); }
            if (game.Status != GameStatuses.Waiting) { throw new Exception("Wrong Game"); }
            if (ships.Count() != 42 || ships.Count(c => c == '█') != 16) { throw new Exception("Wrong field"); }
            if (game.PlayersInfo[0].PlayerId == userId && game.GameInfo.FirstUserReady) { throw new Exception("Already placed"); }
            if (game.PlayersInfo.Count() == 2 && game.PlayersInfo[1].PlayerId == userId && game.GameInfo.SecondUserReady) { throw new Exception("Already placed"); }
        }
        #endregion
    }
}
