using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerLauncherBot.Services
{
    public class LoggingService
    {
        private readonly ILogger _logger;
        private readonly DiscordShardedClient _discord;
        //private readonly CommandService _commands;

        public LoggingService(ILogger<LoggingService> logger, DiscordShardedClient discord)
        {
            _discord = discord;
            _logger = logger;

            _discord.Log += OnLogAsync;
        }

        public Task OnLogAsync(LogMessage msg)
       {
            string logText = $"{msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";

            switch (msg.Severity.ToString())
            {
                case "Critical":
                    {
                        _logger.LogCritical(logText);
                        break;
                    }
                case "Warning":
                    {
                        _logger.LogWarning(logText);
                        break;
                    }
                case "Info":
                    {
                        _logger.LogInformation(logText);
                        break;
                    }
                case "Verbose":
                    {
                        _logger.LogInformation(logText);
                        break;
                    }
                case "Debug":
                    {
                        _logger.LogDebug(logText);
                        break;
                    }
                case "Error":
                    {
                        _logger.LogError(logText);
                        break;
                    }
            }
            return Task.CompletedTask;
        }
    }
}
