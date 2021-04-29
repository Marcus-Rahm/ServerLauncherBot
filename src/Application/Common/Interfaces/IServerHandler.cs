using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IServerHandler
    {
        ulong ServerInfoChannel { get; }
        ServerInfo AddManualServer(ServerInfo serverData, ulong? userId = null);
        Task AddServer(string serverInfo);
        Task AddServerConfig(string serverConfig);
        List<Server> GetServers(); 
    }
}
