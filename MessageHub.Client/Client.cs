using MessageHub.Data.Model;
using System.Net.Sockets;

namespace MessageHub.Client
{
    public class Client
    {
        MessageModel messageModel = new MessageModel();
        public TcpClient TCPClient { get; set; }
        public Guid UID { get; set; }
        public Client(TcpClient client)
        {
            TCPClient = client;
            UID = Guid.NewGuid();
            Client = new Client();
            Console.WriteLine($"{DateTime.Now} Client sucessfull logged with {messageModel.UserName}");     
        }
    }
}
