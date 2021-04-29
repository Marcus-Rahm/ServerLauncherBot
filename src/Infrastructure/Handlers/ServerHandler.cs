using Application.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Domain.Entities;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Handlers
{
    public class ServerHandler : IServerHandler
    {
        private readonly IServerLauncherDbContext _context;
        private List<Server> _activeServers;

        public ulong ServerInfoChannel { get; private set; }

        public ServerHandler(IServerLauncherDbContext context, IConfigurationRoot configuration)
        {
            _activeServers = new List<Server>();
            _context = context;

            ServerInfoChannel = ulong.Parse(configuration.GetSection("ServerHandler:ServerListChannel")?.Value);

            RecoverServers();
        }
        

        public ServerInfo AddManualServer(ServerInfo serverData, ulong? userId = null)
        {
            ServerConfig serverConfig = _context.ServerConfigs.FirstOrDefault(s => s.ConfigName == serverData.Config.ConfigName
            || (s.GameType == serverData.Config.GameType && s.GameVersion == serverData.Config.GameVersion));

            if(serverConfig == null)
            {
                var message = "Unable to find server config: ";
                if(string.IsNullOrEmpty(serverData.Config.ConfigName))
                {
                    message += $"{serverData.Config.ConfigName},";
                }

                if (string.IsNullOrEmpty(serverData.Config.GameType))
                {
                    message += $"{serverData.Config.GameType},";
                }

                if (string.IsNullOrEmpty(serverData.Config.GameVersion))
                {
                    message += $"{serverData.Config.GameVersion},";
                }

                throw new Exception(message);
            }

            ServerInfo serverInfo = _context.ServerInfos.FirstOrDefault(s => s.ServerName == serverData.ServerName);
            if(serverInfo == null)
            {
                serverInfo = new ServerInfo()
                {
                    ServerIP = serverData.ServerIP,
                    ServerName = serverData.ServerName,
                    Description = serverData.Description,
                    ServerConfigId = serverConfig.ServerConfigId
                };

                if (userId.HasValue)
                {
                    serverInfo.OwnerId = userId.Value;
                }

                serverInfo.Config = null;

                _context.ServerInfos.Add(serverInfo);
                _context.SaveChanges();

                serverInfo.Config = serverConfig;

                _activeServers.Add(new Server()
                {
                    ServerInfo = serverInfo,
                    IsManualServer = true
                });
            }
            else
            {
                if (serverInfo.OwnerId == userId)
                {
                    var server = _activeServers.FirstOrDefault(s => s.ServerInfo == serverInfo);

                    serverInfo.ServerIP = serverData.ServerIP;
                    serverInfo.ServerName = serverData.ServerName;
                    serverInfo.Description = serverData.Description;
                    serverInfo.ServerConfigId = serverConfig.ServerConfigId;

                    _context.SaveChanges();

                    if (server != null)
                    {
                        server.ServerInfo = serverInfo;
                    }
                    else
                    {
                        _activeServers.Add(new Server()
                        {
                            ServerInfo = serverInfo,
                            IsManualServer = true
                        });
                    }
                }
                else
                {
                    throw new Exception($"A server already exists with the name {serverInfo.ServerName}");
                }
            }

            return serverInfo;
        }

        public async Task AddServer(string serverInfo)
        {
            throw new NotImplementedException();
        }

        public async Task AddServerConfig(string serverConfig)
        {
            throw new NotImplementedException();
        }

        public List<Server> GetServers()
        {
            return _activeServers;
        }

        private void RecoverServers()
        {
            var serverInfos = _context.ServerInfos.Include(s => s.Config).ToList();
            foreach (var serverInfo in serverInfos)
            {
                _activeServers.Add(new Server()
                {
                    ServerInfo = serverInfo
                });
            }
        }
    }
}
