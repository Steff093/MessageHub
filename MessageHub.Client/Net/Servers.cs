using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MessageHub.Client.Net
{
    public class Servers
    {
        private TcpListener _listener;

        public Servers()
        {
            _listener = new TcpListener(IPAddress.Any, 7000);
        }

        public void Start()
        {
            try
            {
                _listener.Start();
                MessagingCenter.Send(this, "LogMessage", "Server started. Listening for incoming connections...");

                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    Task.Run(() => HandleClient(client));
                }
            }
            catch (Exception)
            {

                throw;
            }
}

private void HandleClient(TcpClient client)
{
    try
    {
        if (!client.Connected)
        {
            MessagingCenter.Send(this, "LogMessage", "Client is not connected.");
            return;
        }

        MessagingCenter.Send(this, "LogMessage", "Client connected.");
        NetworkStream stream = client.GetStream();

        byte[] buffer = new byte[256];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            MessagingCenter.Send(this, "LogMessage", "Received: " + data);

            // Echo the data back to the client
            byte[] responseBuffer = Encoding.ASCII.GetBytes(data);
            stream.Write(responseBuffer, 0, responseBuffer.Length);
        }

        client.Close();
        MessagingCenter.Send(this, "LogMessage", "Client disconnected.");
    }
    catch (Exception ex)
    {
        MessagingCenter.Send(this, "LogMessage", "Error handling client: " + ex.Message);
    }
}

public void Stop()
{
    _listener.Stop();
    MessagingCenter.Send(this, "LogMessage", "Server stopped.");
}
    }
}
