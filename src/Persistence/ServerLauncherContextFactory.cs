using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence
{
    public class ServerLauncherContextFactory : IDesignTimeDbContextFactory<ServerLauncherDbContext>
    {
        public ServerLauncherDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServerLauncherDbContext>();
            optionsBuilder.UseSqlite("Data Source=..\\ServerLauncherBot\\test.db;");

            return new ServerLauncherDbContext(optionsBuilder.Options);
        }
    }
}
