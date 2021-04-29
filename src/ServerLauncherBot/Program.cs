using System;

namespace ServerLauncherBot
{
	public class Program
    {
        public static void Main(string[] args)
        {
			try
			{
				new ServerLauncherBot().StartAsync().GetAwaiter().GetResult();
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
			}
        }

    }
}
