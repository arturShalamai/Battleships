using Battleships.DAL;
using Battleships.DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.DAL
{
    public class BattleshipsContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameInfo> GamesInfo { get; set; }
        public DbSet<GamePlayer> GamePlayer { get; set; }
        public DbSet<PlayerCredentials> Credentials { get; set; }

        public BattleshipsContext(DbContextOptions opts) : base(opts)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<PlayerCredentials>(new PlayerCredentialsConfiguration());

            modelBuilder.ApplyConfiguration<GamePlayer>(new GamePlayerConfiguration());

            modelBuilder.ApplyConfiguration<GameInfo>(new GameInfoConfigurations());

            modelBuilder.ApplyConfiguration<Game>(new GameConfigurations());


            base.OnModelCreating(modelBuilder); 
        }
    }
}
