using MessageHub.Client.Net;
using MessageHub.ViewModel.ViewModels;

namespace MessageHub
{
    public partial class MainPage : ContentPage
    {
        private Servers _server;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }

}
