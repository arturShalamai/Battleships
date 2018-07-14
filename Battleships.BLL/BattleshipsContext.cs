using Battleships.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.BLL
{
    public class BattleshipsContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameInfo> GamesInfo { get; set; }

        public BattleshipsContext(DbContextOptions opts) : base(opts)
        {

        }
    }
}
