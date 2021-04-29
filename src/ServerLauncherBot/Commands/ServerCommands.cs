using Application.Common.Interfaces;
using Discord.Commands;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerLauncherBot.Infrastructure.Handler;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerLauncherBot.Modules.Server
{
    public class ServerCommands : ModuleBase
    {
        private readonly ILogger _logger;
        private readonly IServerHandler _serverHandler;
        public ServerCommands(ILogger<ServerCommands> logger, IServerHandler serverHandler)
        {
            _logger = logger;
            _serverHandler = serverHandler;

        }

        [Command("Add-Server", RunMode = RunMode.Async)]
        [Summary("Prints file content")]
        public async Task AddServer([Remainder] string parameters = null)
        {
            ServerInfo serverInfo = null;

            if (Context.Message.Attachments.Any())
            {
                var attachments = Context.Message.Attachments;

                // Create a new WebClient instance.
                WebClient myWebClient = new WebClient();

                string file = attachments.ElementAt(0).Filename;
                string url = attachments.ElementAt(0).Url;

                // Download the resource and load the bytes into a buffer.
                byte[] buffer = myWebClient.DownloadData(url);

                // Encode the buffer into UTF-8
                string download = Encoding.UTF8.GetString(buffer);

                serverInfo = JsonConvert.DeserializeObject<ServerInfo>(download);

                serverInfo = _serverHandler.AddManualServer(serverInfo, Context.Message.Author.Id);
            }
            else
            {
                var args = parameters.Split('|');
                if(args.Length == 4)
                {
                    serverInfo = new ServerInfo()
                    {
                        ServerName = args.ElementAtOrDefault(0),
                        ServerIP = args.ElementAtOrDefault(1),
                        Description = args.ElementAtOrDefault(2)
                    };

                    if (args.ElementAtOrDefault(2).Contains('-'))
                    {
                        var configInfo = args[3].Split('-');
                        serverInfo.Config = new ServerConfig()
                        {
                            GameType = configInfo[0],
                            GameVersion = configInfo[1]
                        };
                    }
                    else
                    {
                        serverInfo.Config = new ServerConfig()
                        {
                            ConfigName = args.ElementAtOrDefault(3)
                        };
                    }

                    serverInfo = _serverHandler.AddManualServer(serverInfo);
                }
                else
                {
                    throw new Exception("Invalid arguments! Expecting: !Add-Server [ServerName] [ServerIP] [Description] [ConfigName or GameType-GameVersion]");
                }
            }

            await _serverHandler.WriteServerInfo(Context.Client);

            // Clean up message
            await Context.Channel.DeleteMessageAsync(Context.Message);

            await Context.Channel.SendMessageAsync($"Added Server: {serverInfo.ServerName}");
        }
    }
}
