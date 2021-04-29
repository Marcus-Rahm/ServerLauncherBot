
namespace Domain.Entities
{
    public class ServerConfig
    {
        public ServerConfig()
        {

        }

        public int ServerConfigId { get; set; }
        public string ConfigName { get; set; }
        public string GameVersion { get; set; }
        public string ThumbnailURL { get; set; }
        public string ServerFileURL { get; set; }
        public string GameType { get; set; }
    }
}
