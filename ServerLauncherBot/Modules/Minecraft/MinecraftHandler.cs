using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLauncherBot.Modules.Minecraft
{
    public class MinecraftHandler
    {
        private const string _workingDir = "Minecraft";

        private IConfigurationRoot _config;
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;

        public MinecraftHandler(IServiceProvider services)
        {
            _services = services;
            _config = services.GetRequiredService<IConfigurationRoot>();
            _logger = services.GetRequiredService<ILogger<MinecraftHandler>>();
        }

        public void DownloadServer()
        {

        }
    }
}
