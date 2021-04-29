using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class ServerConfigConfiguration : IEntityTypeConfiguration<ServerConfig>
    {
        public void Configure(EntityTypeBuilder<ServerConfig> builder)
        {
            builder.Property(e => e.ServerConfigId)
                .HasColumnName("ServerInfoId");

            builder.Property(e => e.ConfigName)
                .HasColumnName("ConfigName");

            builder.Property(e => e.GameVersion)
                .HasColumnName("GameVersion");

            builder.Property(e => e.ServerFileURL)
                .HasColumnName("ServerFileURL");

            builder.Property(e => e.GameType)
                .HasColumnName("GameType");
        }
    }
}
