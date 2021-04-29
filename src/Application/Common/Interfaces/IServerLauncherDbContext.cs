using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IServerLauncherDbContext
    {
        DbSet<ServerInfo> ServerInfos { get; set; }
        DbSet<ServerConfig> ServerConfigs { get; set; }
        int SaveChanges();
    }
}
