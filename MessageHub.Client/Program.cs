using MessageHub.Data.Model;
using System.Net.Sockets;

namespace MessageHub.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient();
            MessageModel messageModel = new MessageModel { UserName = "JohnDoe" };
            Clients client = new Clients(tcpClient, messageModel);

            Console.WriteLine("Client startet");
        }
    }
}
