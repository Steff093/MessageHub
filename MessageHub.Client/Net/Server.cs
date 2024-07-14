using System.Net.Sockets;

namespace MessageHub.Client.Net
{
    public class Server
    {
        TcpClient _client;

        public Server()
        {
            _client = new TcpClient();
        }
    }
}
