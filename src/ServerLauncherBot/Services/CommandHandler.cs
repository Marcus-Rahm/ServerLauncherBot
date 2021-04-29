using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerLauncherBot.Services
{
    public class CommandHandler
    {
        private CommandService _commands;
        private DiscordShardedClient _discord;
        private readonly IConfigurationRoot _config;
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            _services = services;
            _config = services.GetRequiredService<IConfigurationRoot>();
            _discord = services.GetRequiredService<DiscordShardedClient>();
            _commands = services.GetRequiredService<CommandService>();
            _commands.CommandExecuted += OnCommandExecutedAsync;
            _discord.MessageReceived += HandleCommandAsync;
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
        }

        public async Task HandleCommandAsync(SocketMessage parameterMessage)
        {
            // Don't handle the command if it is a system message
            var message = parameterMessage as SocketUserMessage;
            if (message == null) return;

            // Don't listen to bots
            if (message.Source != MessageSource.User)
            {
                return;
            }

            // Mark where the prefix ends and the command begins
            int argPos = 0;

            // Create a Command Context
            var context = new ShardedCommandContext(_discord, message);

            char prefix = Char.Parse(_config["prefix"]);

            // Determine if the message has a valid prefix, adjust argPos
            if (!(message.HasMentionPrefix(_discord.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos))) return;

            // Execute the Command, store the result            
            var result = await _commands.ExecuteAsync(context, argPos, _services);

            await LogCommandUsage(context, result);
            // If the command failed, notify the user
            if (!result.IsSuccess)
            {
                if (result.ErrorReason != "Unknown command.")
                {
                    await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
                }
            }
        }

        public async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // We have access to the information of the command executed,
            // the context of the command, and the result returned from the
            // execution in this event.

            // We can tell the user what went wrong
            if (!string.IsNullOrEmpty(result?.ErrorReason))
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }

            // ...or even log the result (the method used should fit into
            // your existing log handler)
            var commandName = command.IsSpecified ? command.Value.Name : "A command";
            _logger.LogInformation($"{commandName} was executed at {DateTime.UtcNow} by {context.User.Username}.");
        }

        private async Task LogCommandUsage(SocketCommandContext context, IResult result)
        {
            await Task.Run(async () =>
            {
                if (context.Channel is IGuildChannel)
                {
                    var logTxt = $"User: [{context.User.Username}]<->[{context.User.Id}] Discord Server: [{context.Guild.Name}] -> [{context.Message.Content}]";
                    _logger.LogInformation(logTxt);
                }
                else
                {
                    var logTxt = $"User: [{context.User.Username}]<->[{context.User.Id}] -> [{context.Message.Content}]";
                    _logger.LogInformation(logTxt);
                }
            });
        }
    }
}
