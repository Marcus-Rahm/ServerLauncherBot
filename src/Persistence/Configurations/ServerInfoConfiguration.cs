using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ServerInfoConfiguration : IEntityTypeConfiguration<ServerInfo>
    {
        public void Configure(EntityTypeBuilder<ServerInfo> builder)
        {
            builder.Property(e => e.ServerInfoId)
                .HasColumnName("ServerInfoId");

            builder.Property(e => e.OwnerId)
                .HasColumnName("OwnerId");

            builder.Property(e => e.Description)
                .HasColumnName("Description");

            builder.Property(e => e.OwnerId)
                .HasColumnName("IsRunning");

            builder.Property(e => e.OwnerId)
                .HasColumnName("IsInitialized");

            builder.Property(e => e.OwnerId)
                .HasColumnName("IsArchived");

            builder.Property(e => e.OwnerId)
                .HasColumnName("LastActive").HasColumnType("datetime");

            builder.Property(e => e.OwnerId)
                .HasColumnName("ArchiveFolder");

            builder.HasOne(d => d.Config)
                .WithMany().HasForeignKey(d => d.ServerConfigId);
        }
    }
}
