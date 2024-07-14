using MessageHub.Client.Net;

namespace MessageHub.Server
{
    // All the code in this file is included in all platforms.
    public class Program
    {
        static void Main(string[] args)
        {
            Servers server = new Servers();
            server.Start();

            Console.WriteLine("Server started!");
        }
    }
}
