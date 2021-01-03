using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerLauncherBot.Modules.Testing
{
    public class TestCommands : ModuleBase
    {
        private DiscordShardedClient _discord;
        private readonly IConfigurationRoot _config;
        private string _prefix;
        private readonly ILogger _logger;

        public TestCommands(IServiceProvider services)
        {
            _logger = services.GetRequiredService<ILogger<TestCommands>>();
            _discord = services.GetRequiredService<DiscordShardedClient>();
            _config = services.GetRequiredService<IConfigurationRoot>();
            _prefix = _config["prefix"];
        }

        [Command("Echo", RunMode = RunMode.Async)]
        [Summary("Test function that returns what is writen in args")]
        public async Task Test([Remainder] string args)
        {
            await Context.Channel.SendMessageAsync(args);
        }
    }
}
