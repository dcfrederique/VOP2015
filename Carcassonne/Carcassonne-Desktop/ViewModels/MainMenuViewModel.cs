using System.Windows;
using Carcassonne_Desktop.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Carcassonne_Desktop.Models.Games;
using System;

namespace Carcassonne_Desktop.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        public RelayCommand<Window> SinglePlayerClickedCommand { get; set; }
        public RelayCommand<Window> MultiPlayerClickedCommand { get; set; }

        public MainMenuViewModel()
        {
            InitializeCommand();
        }

        private void InitializeCommand()
        {
            SinglePlayerClickedCommand = new RelayCommand<Window>(NavigateGameSettingView);
            MultiPlayerClickedCommand = new RelayCommand<Window>(NavigateLoginView);
        }

        private void NavigateGameSettingView(Window mainWindow)
        {
           /* BoardView b = new BoardView();
            BoardViewModel bm = new BoardViewModel(new OfflineGame());
            b.DataContext = bm;
            b.Closing += bm.OnWindowsClosing;
            bm.window = b;
            b.Owner = mainWindow;
            b.Show();
            mainWindow.Hide();*/
            GameSettingsView gView = new GameSettingsView {Owner = mainWindow};
            GameSettingsViewModel gm = new GameSettingsViewModel(new OfflineGame());
            gm.Window = gView;
            gView.DataContext = gm;
            gView.Closing += gm.OnWindowsClosing;
            gView.Owner = mainWindow;
            gView.Show();
            mainWindow.Hide();

        }

        private void NavigateLoginView(Window mainWindow)
        {
            LoginView b = new LoginView();
            LoginViewModel bm = (LoginViewModel)b.DataContext;
            bm.Window = b;
            b.Show();
            mainWindow.Hide();
        }

    }
}