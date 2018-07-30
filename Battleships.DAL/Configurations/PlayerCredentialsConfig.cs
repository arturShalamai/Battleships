using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.DAL.Configurations
{
    class PlayerCredentialsConfiguration : IEntityTypeConfiguration<PlayerCredentials>
    {
        public void Configure(EntityTypeBuilder<PlayerCredentials> builder)
        {
            builder.HasOne<Player>(p => p.Player)
                   .WithOne(pc => pc.Credentials)
                   .HasPrincipalKey<PlayerCredentials>(pc => pc.PlayerId);
        }
    }
}
