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

        public IRepository<Player> PlayerRepo { get => _playerRepo ?? new GenericRepo<Player>(_context); }
        public IRepository<Game> GameRepo { get => _gameRepo ?? new GenericRepo<Game>(_context); }
        public IRepository<GameInfo> GameInfo { get => _gameInfo ?? new GenericRepo<GameInfo>(_context); }

        private readonly BattleshipsContext _context;

        public UnitOfWork(BattleshipsContext context)
        {
            _context = context;
        }

        public void SaveAsync()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
