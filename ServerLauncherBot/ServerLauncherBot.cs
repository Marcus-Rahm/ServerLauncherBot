using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ServerLauncherBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerLauncherBot
{
    public class ServerLauncherBot
    {
        private IConfigurationRoot _config;

        public async Task StartAsync()
        {
            //Create configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json");
            _config = builder.Build();

            //Configure services
            var services = new ServiceCollection()
                .AddSingleton(new DiscordShardedClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Debug,
                    MessageCacheSize = 1000
                }))
                .AddSingleton(_config)
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Async,
                    LogLevel = LogSeverity.Verbose,
                    CaseSensitiveCommands = false,
                    ThrowOnError = false
                }))
                .AddSingleton<CommandHandler>()
                .AddSingleton<DiscordStartupService>()
                .AddSingleton<LoggingService>();

            //Add logging
            ConfigureServices(services);

            //Build services
            var serviceProvider = services.BuildServiceProvider();

            //Instantiate logger/tie-in logging
            serviceProvider.GetRequiredService<LoggingService>();

            //Start bot
            await serviceProvider.GetRequiredService<DiscordStartupService>().StartAsync();

            //Load up services
            serviceProvider.GetRequiredService<CommandHandler>();

            //Block this program until it is closed.
            await Task.Delay(-1);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            //Add SeriLog
            services.AddLogging(configure => configure.AddSerilog());
            //Configure logging level
            var logLevel = Environment.GetEnvironmentVariable("LOG_LEVEL");
            var level = Serilog.Events.LogEventLevel.Error;
            if(!string.IsNullOrEmpty(logLevel))
            {
                switch (logLevel.ToLower())
                {
                    case "error":
                        {
                            level = Serilog.Events.LogEventLevel.Error;
                            break;
                        }
                    case "info":
                        {
                            level = Serilog.Events.LogEventLevel.Information;
                            break;
                        }
                    case "debug":
                        {
                            level = Serilog.Events.LogEventLevel.Debug;
                            break;
                        }
                    case "crit":
                        {
                            level = Serilog.Events.LogEventLevel.Fatal;
                            break;
                        }
                    case "warn":
                        {
                            level = Serilog.Events.LogEventLevel.Warning;
                            break;
                        }
                    case "trace":
                        {
                            level = Serilog.Events.LogEventLevel.Debug;
                            break;
                        }
                }
            }

            Log.Logger = new LoggerConfiguration()
                    .WriteTo.File("logs/serverlauncherbot.log", rollingInterval: RollingInterval.Day)
                    .WriteTo.Console()
                    .MinimumLevel.Is(level)
                    .CreateLogger();
        }
    }
}
