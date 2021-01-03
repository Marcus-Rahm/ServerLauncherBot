using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServerLauncherBot.Database
{
    public partial class MinecraftVersion
    {
        [Key]
        public long Id { get; set; }
        public string Version { get; set; }
        public string Url { get; set; }
        public string FileLocation { get; set; }

    }
}
