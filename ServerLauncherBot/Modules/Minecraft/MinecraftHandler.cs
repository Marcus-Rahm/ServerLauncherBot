using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServerLauncherBot.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

        public async Task AddMinecraftVersion(string version, string url)
        {
            using (var db = new ServerLauncherEntities())
            {
                if (!db.MinecraftVersion.Any(mv => mv.Version == version))
                {
                    db.MinecraftVersion.Add(new MinecraftVersion
                    {
                        Version = version,
                        Url = url
                    });
                    await db.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Version: {version} already exists");
                }
            }
        }

        public async Task<MinecraftVersion> DownloadServer(string version)
        {
            MinecraftVersion minecraftVersion = null;
            using (var db = new ServerLauncherEntities())
            {
                minecraftVersion = db.MinecraftVersion.FirstOrDefault(mv => mv.Version == version);

                if (minecraftVersion != null)
                {
                    if (string.IsNullOrWhiteSpace(minecraftVersion.FileLocation) || !File.Exists(minecraftVersion.FileLocation))
                    {
                        minecraftVersion.FileLocation = Path.Combine(AppContext.BaseDirectory, _workingDir, "ServerFiles",$"{minecraftVersion.Version}.jar");
                        using (var client = new WebClient())
                        {
                            if (!File.Exists(minecraftVersion.FileLocation))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(minecraftVersion.FileLocation));

                                await Task.Run(() => client.DownloadFile(new Uri(minecraftVersion.Url), minecraftVersion.FileLocation));
                            }
                        }

                        db.SaveChanges();
                    }

                    return minecraftVersion;
                }
                else
                {
                    throw new Exception($"Error: No valid minecraft version found for: {version}");
                }
            }
        }
    }
}
