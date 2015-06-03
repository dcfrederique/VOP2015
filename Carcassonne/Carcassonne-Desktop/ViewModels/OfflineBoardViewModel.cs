using System;
using Carcassonne_Desktop.Models;
using Carcassonne_Desktop.Models.Games;
using Carcassonne_Desktop.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;

namespace Carcassonne_Desktop.ViewModels
{
    public class OfflineBoardViewModel : ViewModelBase
    {
        private TileViewer tileViewer;
        private TileViewerModel tileViewerModel;
        public OfflineBoardView window;

        public OfflineBoardViewModel(Game game)
        {
            InitializeCommands();
            Game = game;
            tileViewer = new TileViewer();
            tileViewerModel = (TileViewerModel)tileViewer.DataContext;
            tileViewerModel.Window = tileViewer;
            tileViewerModel.Game = Game;
            game.PropertyChanged+=Game_finished;
        }

        private void Game_finished(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "isFinished")
            {
                ScoreBoardView scoreBoardView = new ScoreBoardView();
                scoreBoardView.Owner = window;
                var sbv = (ScoreBoardModel)scoreBoardView.DataContext;
                sbv.Window = scoreBoardView;
                sbv.Players = Game.Players;
                scoreBoardView.ShowDialog();    
                
            }
        }

        public Game Game { get; set; }
        public RelayCommand<String> RotateCommand { get; set; }
        public RelayCommand<Tile> AddBtnCommand { get; set; }
        public RelayCommand ReportAbuseCommand { get; set; }

        private void InitializeCommands()
        {
            RotateCommand = new RelayCommand<String>(RotateTile);
            AddBtnCommand = new RelayCommand<Tile>(AddPossibleTiles);
            ReportAbuseCommand = new RelayCommand(ReportAbuse);
        }

        private void ReportAbuse()
        {
            //TEST 
            Process.Start(new ProcessStartInfo(new Uri(resources.URLResource.ReportAbuseURL + "510e5c21-ed52-4aa8-9613-475c63828bdf").AbsoluteUri));
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
            SoundPlayer audioPlayer = new SoundPlayer(Properties.Resources.Rotation);
            audioPlayer.Play();
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
            tileViewerModel.Window.Close();
            window.Owner.Show();
        }
    }
}