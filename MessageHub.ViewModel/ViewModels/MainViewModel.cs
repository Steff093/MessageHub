using MessageHub.Client;
using MessageHub.Client.Net;
using MessageHub.Data.Model;
using MessageHub.ViewModel.Core;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace MessageHub.ViewModel.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Servers _server;
        private Clients _client;
        private bool _isServerRunning;
        private string _serverStatus;
        private string _connectionStatus;
        private string _messageToSend;

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

        private string _logMessages;

        public string LogMessages
        {
            get => _logMessages;
            set
            {
                _logMessages = value;
                OnPropertyChanged();
            }
        }

        public bool IsServerRunning
        {
            get { return _isServerRunning; }
            set
            {
                if (_isServerRunning != value)
                {
                    _isServerRunning = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ServerStatus
        {
            get => _serverStatus;
            set
            {
                _serverStatus = value;
                OnPropertyChanged();
            }
        }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        public string MessageToSend
        {
            get => _messageToSend;
            set
            {
                _messageToSend = value;
                OnPropertyChanged();
            }
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
