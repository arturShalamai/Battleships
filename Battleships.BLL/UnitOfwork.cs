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
        private IRepository<PlayerConnection> _playerConnections;
        private IRepository<GamesConnection> _gameConnection;

        public IRepository<Player> PlayerRepo { get => _playerRepo ?? new GenericRepo<Player>(_context); }
        public IRepository<Game> GameRepo { get => _gameRepo ?? new GenericRepo<Game>(_context); }
        public IRepository<GameInfo> GameInfoRepo { get => _gameInfo ?? new GenericRepo<GameInfo>(_context); }
        public IRepository<PlayerConnection> PlayerConnections { get => _playerConnections ?? new GenericRepo<PlayerConnection>(_context); }
        public IRepository<GamesConnection> GameConnections { get => _gameConnection ?? new GenericRepo<GamesConnection>(_context); }

        private readonly BattleshipsContext _context;

        public UnitOfWork(BattleshipsContext context)
        {
            _context = context;
        }

        public void Save()
        {
             _context.SaveChanges();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
