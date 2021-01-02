using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLauncherBot.Services
{
    public class ServerHandler
    {
        private readonly IConfigurationRoot _config;
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        public ServerHandler(IServiceProvider services)
        {
            _services = services;
            _config = services.GetRequiredService<IConfigurationRoot>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
        }


    }
}
