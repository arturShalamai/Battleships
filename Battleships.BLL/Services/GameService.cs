using System;
using System.Collections.Generic;
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

        public async Task<int> StartGameAsync()
        {
            var player = await _unit.PlayerRepo.SingleAsync(_ => true);
            var game = new DAL.Game(player);
            await _unit.GameRepo.AddAsync(game);

            await _unit.SaveAsync();

            return game.Id;
        }
    }
}
