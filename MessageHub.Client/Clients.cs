using MessageHub.Data.Model;
using System.Net.Sockets;
using System.Text;

namespace MessageHub.Client
{
    public class Clients
    {
        public MessageModel MessageModel { get; set; }
        public TcpClient TCPClient { get; set; }
        public Guid UID { get; set; }

        public Clients(TcpClient client, MessageModel messageModel)
        {
            TCPClient = client ?? throw new ArgumentNullException(nameof(client));
            MessageModel = messageModel ?? throw new ArgumentNullException(nameof(messageModel));
            UID = Guid.NewGuid();
            MessagingCenter.Send(this, $"{DateTime.Now} Client successfully logged with {MessageModel.UserName}");
        }

        public void ConnectToServer()
        {
            try
            {
                TCPClient.Connect("127.0.0.1", 7000);
                MessagingCenter.Send(this, "Connected to server.");

                // Send a message to the server
                NetworkStream stream = TCPClient.GetStream();
                string message = "Hello, Server!";
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // Receive the response from the server
                data = new byte[256];
                int bytesRead = stream.Read(data, 0, data.Length);
                string response = Encoding.ASCII.GetString(data, 0, bytesRead);
                MessagingCenter.Send(this, "Server response: {0}", response);

                // Close the connection
                TCPClient.Close();
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(this, "Failed to connect to server: " + ex.Message);
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                if (TCPClient.Connected)
                {
                    NetworkStream stream = TCPClient.GetStream();
                    byte[] buffer = Encoding.ASCII.GetBytes(message);
                    stream.Write(buffer, 0, buffer.Length);
                    Console.WriteLine("Message sent to server.");
                }
                else
                {
                    Console.WriteLine("Client is not connected to the server.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send message: " + ex.Message);
            }
        }
    }
}
