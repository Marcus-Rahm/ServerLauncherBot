using Application.Common.Interfaces;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLauncherBot.Infrastructure.Handler
{
    public static class ServerHandlerExtenstions
    {
        public static async Task WriteServerInfo(this IServerHandler serverHandler, IDiscordClient client)
        {
            var channel = await client.GetChannelAsync(serverHandler.ServerInfoChannel) as ITextChannel;
            var messages = (await channel.GetMessagesAsync().FlattenAsync()).ToList();

            var servers = serverHandler.GetServers();

            if(servers.Count < messages.Count() && messages.Any(m => (DateTimeOffset.UtcNow - m.Timestamp).TotalDays > 14))
            {
                //Recreate channel because discord does not allow deletion of messages older then 2 weeks
                var guild = channel.Guild;
                await channel.DeleteAsync();
                channel = await guild.CreateTextChannelAsync(channel.Name,
                    p => p.CategoryId = channel.CategoryId);

                messages.Clear();
            }
            else
            {
                var messagesToDelete = messages.Skip(servers.Count);
                await channel.DeleteMessagesAsync(messagesToDelete);
                messages = messages.Take(servers.Count).ToList();
            }

            for (int i = 0; i < servers.Count; i++)
            {
                var server = servers[i];
                var embed = await BuildServerInfoEmbed(server, client);

                var message = messages.ElementAtOrDefault(i);

                if (message != null)
                {
                    await (message as RestUserMessage).ModifyAsync(m => m.Embed = embed);
                }
                else
                {
                    await channel.SendMessageAsync(null, embed: embed);
                }
            }
        }

        private static async Task<Embed> BuildServerInfoEmbed(Server server, IDiscordClient client)
        {
            var ManualOrLaunched = server.IsManualServer ? " `📃Manual server`" : " `🚀Launched server`";
            var serverInfo = server.ServerInfo;

            var random = new Random();
            var color = new Color(random.Next(255), random.Next(255), random.Next(255));

            var user = await client.GetUserAsync(server.ServerInfo.OwnerId);
            var username = user == null ? "Missing" : user.Username;
            var Icon = user == null ? "" : user.GetAvatarUrl();


            var builder = new EmbedBuilder()
            .WithTitle(serverInfo.ServerName + ManualOrLaunched)
            .WithDescription(serverInfo.Description)
            .WithColor(color)
            .WithImageUrl(serverInfo.Config.ThumbnailURL)
            .WithAuthor(author =>
            {
                author.WithName(username);
                author.IconUrl = Icon;
            })
            .AddField("Game:", serverInfo.Config.GameType)
            .AddField("Version:", serverInfo.Config.GameVersion)
            .AddField("Server IP:", serverInfo.ServerIP);

            return builder.Build();
        }
    }
}
