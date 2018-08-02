using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.DAL.Configurations
{
    public class GameInfoConfigurations : IEntityTypeConfiguration<GameInfo>
    {
        public void Configure(EntityTypeBuilder<GameInfo> builder)
        {
            builder.HasKey(gi => gi.GameId);

            builder.HasOne(gi => gi.Game)
                .WithOne(g => g.GameInfo)
                .HasPrincipalKey<Game>(g => g.Id);

            builder.HasOne(i => i.FirstPlayer)
                .WithMany()
                .HasForeignKey(i => i.FirstPlayerId);

            builder.HasOne(i => i.SecondPlayer)
                .WithMany()
                .HasForeignKey(i => i.SecondPlayerId);
        }
    }
}
