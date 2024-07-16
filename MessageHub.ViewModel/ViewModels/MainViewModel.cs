
using CommunityToolkit.Mvvm.ComponentModel;
using MessageHub.Client;
using MessageHub.Client.Net;
using MessageHub.Data.Model;
using MessageHub.ViewModel.Core;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace MessageHub.ViewModel.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private Servers _server;
        [ObservableProperty]
        private Clients _client;
        [ObservableProperty]
        private bool _isServerRunning;
        [ObservableProperty]
        private string _serverStatus;
        [ObservableProperty]
        private string _connectionStatus;
        [ObservableProperty]
        private string _messageToSend;
        [ObservableProperty]
        private string _logMessages;

        public RelayCommand StartServerCommand { get; set; }
        public RelayCommand StopServerCommand { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        public MainViewModel()
        {

            MessagingCenter.Subscribe<Servers, string>(this, "LogMessage", (sender, arg) =>
            {
                LogMessages += arg + Environment.NewLine;
            });

            TcpClient tcpClient = new TcpClient();
            MessageModel messageModel = new MessageModel { UserName = "JohnDoe" };
            _client = new Clients(tcpClient, messageModel);
            _server = new Servers();

            StartServerCommand = new RelayCommand(o => StartServer(), o => !IsServerRunning);
            StopServerCommand = new RelayCommand(o => StopServer(), o => IsServerRunning);
            ConnectToServerCommand = new RelayCommand(o => ConnectToServer(), o => !IsServerRunning);
            SendMessageCommand = new RelayCommand(o => SendMessage(), o => !string.IsNullOrEmpty(MessageToSend));

            ServerStatus = "Stopped";
            ConnectionStatus = "Disconnected";
        }

        private void ConnectToServer()
        {
            Task.Run(() =>
            {
                _client.ConnectToServer();
                ConnectionStatus = "Connected";
            });
        }

        private void StartServer()
        {
            Task.Run(() =>
            {
                _server.Start();
                ServerStatus = "Running";
                IsServerRunning = true;
            });
        }

        private void StopServer()
        {
            _server.Stop();
            ServerStatus = "Stopped";
            IsServerRunning = false;
        }

        private void SendMessage()
        {
            Task.Run(() =>
            {
                // Implement the logic to send the message to the server
                _client.SendMessage(MessageToSend);
                LogMessages += $"Sent: {MessageToSend}\n";
                MessageToSend = string.Empty;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
