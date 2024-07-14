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

        public RelayCommand StartServerCommand { get; set; }
        public RelayCommand StopServerCommand { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }

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
        public void LogMessage(string message)
        {
            LogMessages += message + Environment.NewLine;
        }
        private void ConnectToServer()
        {
            Task.Run(() =>
            {
                _client.ConnectToServer();
                IsServerRunning = true;
            });
        }

        private void StartServer()
        {
            Task.Run(() =>
            {
                _server.Start();
                IsServerRunning = true;
            });
        }

        private void StopServer()
        {
            _server.Stop();
            IsServerRunning = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
