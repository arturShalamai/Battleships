using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.DAL.Configurations
{
    class GameConnectionConfigurations : IEntityTypeConfiguration<GamesConnection>
    {
        public void Configure(EntityTypeBuilder<GamesConnection> builder)
        {
            builder.HasKey(gc => gc.Id);

            builder.HasOne(gc => gc.Game).WithMany().HasForeignKey(gc => gc.GameId);

            builder.HasOne(gc => gc.Player).WithOne();
        }
    }
}
