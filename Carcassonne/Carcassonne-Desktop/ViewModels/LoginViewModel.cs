using System.Windows;
using Carcassonne_Desktop.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System;
using Carcassonne_Desktop.Models.NetModels;
using Carcassonne_Desktop.Models.Games;
namespace Carcassonne_Desktop.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        public RelayCommand<object> LoginClickedCommand { get; set; }
        public RelayCommand RegisterClickedCommand { get; set; } 

        public string Username { get; set; }
        private string warningMessage;
        public string WarningMessage
        {
            get { return warningMessage; }
            set { Set(() => WarningMessage, ref warningMessage, value);  }
        }

        private LoginService loginservice;

        public Window Window { get; set; }

        public LoginViewModel()
        {
            InitializeCommand();
        }

        private void InitializeCommand()
        {
            LoginClickedCommand = new RelayCommand<object>(CheckLogin);
            RegisterClickedCommand = new RelayCommand(NavigateToRegistration);
        }

        private void NavigateToRegistration()
        {
            Process.Start(new ProcessStartInfo(new Uri(resources.URLResource.RegisterURL).AbsoluteUri));
        }

        private void CheckLogin(object p)
        {
            var pbox = p as System.Windows.Controls.PasswordBox;

            loginservice = new LoginService();

            if (loginservice.Login(Username, pbox.Password))
            {
                UserState._getInstance().GetUser(loginservice.Token);
                NavigateBoardView();
                Window.Close();
            }
            else
            {
                WarningMessage = "Verkeerde gebruikersnaam/wachtwoord!";
            }
        }

        private void NavigateBoardView()
        {
            WebGame web = new WebGame("0");
            OnlineBoardView b = new OnlineBoardView();
            OnlineBoardViewModel bm = new OnlineBoardViewModel(web);
            b.DataContext = bm;
            b.Closing += bm.OnWindowsClosing;
            bm.window = b;
            //b.Owner = Window;
            b.Show();
        }



    }
}