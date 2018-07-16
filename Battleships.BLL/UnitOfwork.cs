using Battleships.BLL.Repos;
using Battleships.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.BLL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private IRepository<Player> _playerRepo;
        private IRepository<Game> _gameRepo;
        private IRepository<GameInfo> _gameInfo;

        public IRepository<Player> PlayerRepo
        {
            get
            {
                if (_playerRepo == null) { _playerRepo = new GenericRepo<Player>(_context); }
                return _playerRepo;
            }
        }
        public IRepository<Game> GameRepo { get => GameRepo ?? new GenericRepo<Game>(_context); }
        public IRepository<GameInfo> GameInfo { get => GameInfo ?? new GenericRepo<GameInfo>(_context); }

        private readonly BattleshipsContext _context;

        public UnitOfWork(BattleshipsContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
