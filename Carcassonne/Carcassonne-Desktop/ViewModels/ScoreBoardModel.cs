using Carcassonne_Desktop.Models;
using Carcassonne_Desktop.Models.Games;
using Carcassonne_Desktop.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassonne_Desktop.ViewModels
{
    public class ScoreBoardModel: ViewModelBase
    {
        public ScoreBoardView window;
        private List<Player> players;

        public RelayCommand<String> CloseCommand { get; set; }

        public ScoreBoardModel()
        {
            InitializeCommand();
        }

        private void InitializeCommand()
        {
            CloseCommand = new RelayCommand<String>(EndGame);
        }

        private void EndGame(string obj)
        {
            Window.Owner.Close();
        }

   
        public ScoreBoardView Window
        {
            get { return window; }
            set { Set(() => Window, ref window, value); }
        }
        public List<Player> Players
        {
            get
            {
                return players;
            }
            set { Set(() => Players, ref players, value); }
        }






    }
}
