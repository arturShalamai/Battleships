using AutoMapper;
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
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<List<Guid>> GetAllGames()
        {
            var games = await _unit.GameRepo.AllAsync();
            return games.Select(g => g.Id).ToList();
        }

        public async Task<GameInfoModel> GetByIdAsync(Guid gameId, Guid userId)
        {
            var game = await GetGame(gameId);
            if(!HasAccess(game, userId)) { throw new Exception("Wrong game"); }

            var res = _mapper.Map<GameInfoModel>(game);

            if(game.PlayersInfo[0].PlayerId == userId)
            {
                res.PlayerField = game.GameInfo.FirstUserField;
                res.PlayerReady = game.GameInfo.FirstUserReady;

                res.EnemyField = game.GameInfo.SecondUserField.Replace('█', ' ');
                res.EnemyReady= game.GameInfo.SecondUserReady;

                res.Turn = "Player";
            }
            else
            {
                res.PlayerField = game.GameInfo.SecondUserField;
                res.PlayerReady = game.GameInfo.SecondUserReady;

                res.EnemyField = game.GameInfo.FirstUserField.Replace('█', ' ');
                res.EnemyReady = game.GameInfo.FirstUserReady;

                res.Turn = "Enemy";
            }

            return res;
        }

        public async Task JoinAsync(Guid gameId, string userId)
        {
            var game = await GetGame(gameId);
            if (game.Status != GameStatuses.Waiting) { throw new Exception("You cannot place ships at this game"); }

            var currPlayer = await _unit.PlayerRepo.SingleAsync(p => p.Id.ToString() == userId);
            game.AddPlayer(currPlayer);

            await _unit.GameRepo.UpdateOneAsync(game);
            await _unit.SaveAsync();
        }

        public async Task<Guid> StartGameAsync(Guid creatorId)
        {
            var creator = await _unit.PlayerRepo.SingleAsync(p => p.Id == creatorId, c => c.GamesInfo);
            var game = new Game(creator);
            await _unit.GameRepo.AddAsync(game);

            await _unit.SaveAsync();

            return game.Id;
        }

        public async Task PlaceShips(string ships, Guid userId, Guid gameId)
        {
            var game = await GetGame(gameId);
            if(game.Status != GameStatuses.Waiting) { throw new Exception("You cannot place ships at this game"); }

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

            if(game.GameInfo.FirstUserReady && game.GameInfo.SecondUserReady) { game.Status = GameStatuses.Started; }

            await _unit.GameRepo.UpdateOneAsync(game);
            await _unit.SaveAsync();
        }

        public async Task<ShotResult> Shot(Guid gameId, Guid userId, int number)
        {
            var game = await GetGame(gameId);
            if (game.Status != GameStatuses.Started) { throw new Exception("You cannot play this game"); }

            var field = "";

            if (HasAccess(game, userId) && 
                CheckTurn(game, userId) && 
                CheckPlayersReady(game))
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
                    sb[number] = 'x';
                    res = ShotResult.Hit;
                    if (!field.Any(x => x == '█'))
                    {
                        SetWinner(game, userId);
                        res = ShotResult.Win;
                        return res;
                    }
                }
                else
                {
                    sb[number] = '·';
                    res = ShotResult.Miss;

                    game.SwitchTurn();
                }

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

        public async Task Surrender(Guid gameId, Guid userId)
        {
            var game = await GetGame(gameId);

            if (!HasAccess(game, userId)) { throw new Exception("Wrong game"); }

            game.Winner = game.PlayersInfo[0].PlayerId == userId ? true : false;
            game.Status = GameStatuses.Finished;

            await _unit.GameRepo.UpdateOneAsync(game);
            await _unit.SaveAsync();
        }

        public async Task<bool> CheckAccess(Guid gameId, Guid userId)
        {
            var game = await GetGame(gameId);
            return HasAccess(game, userId);
        }

        #region Helpers
        private bool CheckPlayersReady(Game game) =>
            game.GameInfo.FirstUserReady && game.GameInfo.SecondUserReady;

        private bool HasAccess(Game game, Guid userId) =>
            game.PlayersInfo.Any(p => p.PlayerId == userId);

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

        private async Task<Game> GetGame(Guid id) =>
            await _unit.GameRepo.SingleAsync(g => g.Id == id, g => g.PlayersInfo, g => g.GameInfo);

        #endregion
    }
}
