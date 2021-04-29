using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ServerLauncherDbContext : DbContext, IServerLauncherDbContext
    {
        public ServerLauncherDbContext(DbContextOptions<ServerLauncherDbContext> options)
            : base(options)
        {
        }

        public DbSet<ServerInfo> ServerInfos { get; set; }
        public DbSet<ServerConfig> ServerConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServerLauncherDbContext).Assembly);
        }
    }
}
