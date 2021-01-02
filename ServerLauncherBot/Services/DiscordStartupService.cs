using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerLauncherBot.Services
{
    public class DiscordStartupService
    {
        private readonly DiscordShardedClient _discord;
        private readonly IConfigurationRoot _config;
        private readonly IServiceProvider _services;

        public DiscordStartupService(IServiceProvider services)
        {
            _services = services;
            _config = _services.GetRequiredService<IConfigurationRoot>();
            _discord = _services.GetRequiredService<DiscordShardedClient>();
        }

        public async Task StartAsync()
        {
            string discordToken = _config["Token"];
            if(string.IsNullOrWhiteSpace(discordToken))
            {
                throw new Exception("Token missing from config");
            }

            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();
        }
    }
}
