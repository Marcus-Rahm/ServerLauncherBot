using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ServerLauncherDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("ServerLauncherDatabase")));

            services.AddScoped<IServerLauncherDbContext>(provider => provider.GetService<ServerLauncherDbContext>());

            return services;
        }
    }
}
