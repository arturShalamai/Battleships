using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.DAL.Configurations
{
    class PlayerConnectionConfigurations : IEntityTypeConfiguration<PlayerConnection>
    {
        public void Configure(EntityTypeBuilder<PlayerConnection> builder)
        {
            builder.HasKey(pc => pc.Id);

            builder.HasOne(pc => pc.Player)
                .WithMany(p => p.Connections)
                .HasForeignKey(pc => pc.PlayerId);

        }
    }
}
