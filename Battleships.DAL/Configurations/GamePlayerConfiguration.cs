using Battleships.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.DAL.Configurations
{
    class GamePlayerConfiguration : IEntityTypeConfiguration<GamePlayer>
    {
        public void Configure(EntityTypeBuilder<GamePlayer> builder)
        {
            builder.HasKey(g => g.Id);

            builder.HasOne(g => g.Player)
                   .WithMany(p => p.GamesInfo)
                   .HasForeignKey(g => g.PlayerId);

            builder.HasOne(g => g.Game)
                   .WithMany(g => g.PlayersInfo)
                   .HasForeignKey(g => g.GameId);
        }
    }
}
