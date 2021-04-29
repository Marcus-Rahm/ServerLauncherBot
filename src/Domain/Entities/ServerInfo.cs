using System;

namespace Domain.Entities
{
    public class ServerInfo
    {
        public ServerInfo()
        {
        }

        public int ServerInfoId { get; set; }
        public string ServerName { get; set; }
        public string Description { get; set; }
        public ulong OwnerId { get; set; }
        public string ServerIP { get; set; }
        public bool IsRunning { get; set; }
        public bool IsInitialized { get; set; }
        public bool IsArchived { get; set; }
        public DateTime LastActive { get; set; }
        public string ArchiveFolder { get; set; }
        public int ServerConfigId { get; set; }
        public ServerConfig Config { get; set; }

    }
}
