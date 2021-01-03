using ServerLauncherBot.Common;
using ServerLauncherBot.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLauncherBot.Modules.Minecraft.Infrastructure
{
    public class MinecraftServer : ServerBase
    {
        private MinecraftVersion _serverVersion;

        public MinecraftServer(MinecraftVersion serverVersion, string args) : base(serverVersion.FileLocation, args)
        {
            _serverVersion = serverVersion;
        }
    }
}
