using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Domain.Entities
{
    public class Server
    {
        public ServerInfo ServerInfo { get; set; }
        public int CurrentPlayerCount { get; set; }
        public Process ServerProcess { get; set; }
        public bool IsManualServer { get; set; }
    }
}
