using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Carcassonne_Desktop.Models.Games;
using Carcassonne_Desktop.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;


namespace Carcassonne_Desktop.ViewModels
{
    public class GameSettingsViewModel:ViewModelBase
    {
        public RelayCommand<Window> GameStartClickedCommand { get; set; }
        public RelayCommand<Window>CancelBtnClickedCommand { get; set; }
        public ObservableCollection<string> AvatarList { get; set; }
        private string chosenAvatar;
        public string ChosenAvatar
        {
            get { return chosenAvatar; }
            set { Set(() => ChosenAvatar, ref chosenAvatar, value); }
        }

        private string playerName;
        public string PlayerName
        {
            get { return playerName; }
            set { Set(() => PlayerName, ref playerName, value); }
        }

        private int amountOfPlayers;

        public int AmountOfPlayers
        {
            get { return amountOfPlayers; }
            set { Set(() => AmountOfPlayers, ref amountOfPlayers, value); }
        }
        public OfflineGame Game { get; set; }
        public GameSettingsView Window { get; set; }

        public GameSettingsViewModel(OfflineGame game)
        {
            InitializeCommand();
            Game = game;
            //default amount of players
            amountOfPlayers = 1;
            AvatarList = new ObservableCollection<string>
            {
                "..\\resources\\Pawns\\pawn_blu.png",
                "..\\resources\\Pawns\\pawn_blk.png",
                "..\\resources\\Pawns\\pawn_grn.png",
                "..\\resources\\Pawns\\pawn_red.png",
                "..\\resources\\Pawns\\pawn_ylw.png"
            };


        }

        private void InitializeCommand()
        {
            GameStartClickedCommand = new RelayCommand<Window>(NavigateBoardView);
            CancelBtnClickedCommand = new RelayCommand<Window>(CancelBtnClicked);
        }

        private void CancelBtnClicked(Window w)
        {
            w.Close();
            Window.Owner.Show();
        }

        private void NavigateBoardView(Window mainWindow)
        {
            Game.Initialize(playerName,amountOfPlayers,chosenAvatar);
            OfflineBoardView b = new OfflineBoardView();
            OfflineBoardViewModel bm = new OfflineBoardViewModel(Game);
            b.DataContext = bm;
            b.Closing += bm.OnWindowsClosing;
            bm.window = b;
            b.Owner = mainWindow.Owner;
            b.Show();
            mainWindow.Hide();

        }

        public void OnWindowsClosing(object sender, CancelEventArgs e)
        {
            
            Window.Owner.Show();
        }
    }
}
