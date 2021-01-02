using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ServerLauncherBot
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            var token = "k8G0bJ9s1xktZzFgbV4L4fKO4K8jafon";
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
