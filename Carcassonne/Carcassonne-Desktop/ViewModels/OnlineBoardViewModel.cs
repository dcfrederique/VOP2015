using System;
using Carcassonne_Desktop.Models;
using Carcassonne_Desktop.Models.Games;
using Carcassonne_Desktop.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using Carcassonne_Desktop.Models.NetModels.GameModels;

namespace Carcassonne_Desktop.ViewModels
{
    public class OnlineBoardViewModel : ViewModelBase
    {
        private TileViewer tileViewer;
        private TileViewerModel tileViewerModel;
        public OnlineBoardView window;


        public OnlineBoardViewModel(WebGame game)
        {
            InitializeCommands();
            Game = game;
            tileViewer = new TileViewer();
            tileViewerModel = (TileViewerModel)tileViewer.DataContext;
            tileViewerModel.Window = tileViewer;
            tileViewerModel.Game = Game;
        }

        public WebGame Game { get; set; }
        public RelayCommand<String> RotateCommand { get; set; }
        public RelayCommand<Tile> AddBtnCommand { get; set; }

        private void InitializeCommands()
        {
            RotateCommand = new RelayCommand<String>(RotateTile);
            AddBtnCommand = new RelayCommand<Tile>(AddPossibleTiles);
        }

        private void RotateTile(String rotationAngle)
        {
            if (rotationAngle == "left")
            {
                Game.NextTile.Rotate_left();
                Game.NextTile.Angle -= 90;
            }
            else if (rotationAngle == "right")
            {
                Game.NextTile.Rotate_right();
                Game.NextTile.Angle += 90;
            }
            Game.FindAmountOfValidPlaces(Game.NextTile);
        }

        private void AddPossibleTiles(Tile sourceTile)
        {
            if(Game.CanPlaceOnBoard)
            {
                if (sourceTile.Texture == null)
                {
                    Game.NextTile.Location = new Location(sourceTile.Location.X, sourceTile.Location.Y);
                    Game.MergeFeatures();
                    if (Game.CurPlayer.PawnCount > 0)
                    {
                            Game.CanPlaceOnBoard = false;
                            tileViewer.Owner = window;
                            tileViewerModel.UpdateSettings(sourceTile);
                            tileViewer.ShowDialog();
                    }
                    else
                    {
                        Game.PlacePawnOnTile(FeaturePosition.none, sourceTile);
                        if (Game.isFinished)
                        {
                            ScoreBoardView scoreBoardView = new ScoreBoardView();
                            scoreBoardView.Owner = window;
                            var sbv = (ScoreBoardModel)scoreBoardView.DataContext;
                            sbv.Window = scoreBoardView;
                            sbv.Players = Game.Players;
                            scoreBoardView.ShowDialog();    
                        }
                    }
                }
            }   
        }

        public void OnWindowsClosing(object sender, CancelEventArgs e)
        {
            Game.Leave();
            tileViewerModel.Window.Close();
            window.Owner.Show();
        }

    }
}