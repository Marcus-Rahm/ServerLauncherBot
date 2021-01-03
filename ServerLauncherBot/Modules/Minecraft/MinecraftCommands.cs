using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServerLauncherBot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace ServerLauncherBot.Modules.Minecraft
{
    public class MinecraftCommands : ModuleBase
    {

        private readonly IConfigurationRoot _config;
        private readonly ILogger _logger;
        private readonly MinecraftHandler _minecraftHandler;

        public MinecraftCommands(IServiceProvider services)
        {
            _config = services.GetRequiredService<IConfigurationRoot>();
            _logger = services.GetRequiredService<ILogger<MinecraftCommands>>();
            _minecraftHandler = services.GetRequiredService<MinecraftHandler>();
        }

        [Command("add-minecraft-version", RunMode = RunMode.Async)]
        [Summary("Adds Minecraft server version")]
        [RequireOwner]
        public async Task AddMinecraftVersion([Remainder] string args = null)
        {
            if(args != null)
            {
                try
                {
                    var serverArgs = args.Split(' ');
                    if(serverArgs.Count() == 2)
                    {
                        await _minecraftHandler.AddMinecraftVersion(serverArgs[0].Trim(), serverArgs[1].Trim());
                    }
                }
                catch (Exception ex)
                {
                    await Context.Channel.SendMessageAsync($"Error: [{ex.Message}]");
                }
            }
        }

        [Command("minecraft-versions", RunMode = RunMode.Async)]
        [Summary("List all avalible server versions")]
        public async Task MinecraftVersions()
        {
            try
            {
                List<MinecraftVersion> versions = null;
                using (var db = new ServerLauncherEntities())
                {
                    versions = db.MinecraftVersion.ToList();
                }
                if(versions != null)
                {
                    var versionList = string.Join("\n", versions.Select(mv => mv.Version));
                    EmbedBuilder builder = new EmbedBuilder()
                        .WithTitle("Available Minecraft server versions")
                        .WithDescription(versionList);
                    var embed = builder.Build();
                    await Context.Channel.SendMessageAsync(null, embed: embed);
                }
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"Error listing entries: [{ex.Message}]");
            }
        }

        [Command("test-download", RunMode = RunMode.Async)]
        public async Task TestDownload([Remainder] string args)
        {
            await _minecraftHandler.DownloadServer(args);
            await Context.Channel.SendMessageAsync("Downloaded: " + args);

        }
    }
}
