using Network;
using System.Net;
using System.Net.Sockets;

namespace MessageHub.Server
{
    // All the code in this file is included in all platforms.
    public class Program
    {
        static TcpListener _listener {  get; set; }
        static void Main(string[] args)
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.01", "7000"));
            _listener.Start();

            var client = _listener.AcceptTcpClient();
        }
    }
}
