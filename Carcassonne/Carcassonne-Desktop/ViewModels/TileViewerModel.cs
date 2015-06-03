using System;
using System.Collections.Generic;
using Carcassonne_Desktop.Models;
using Carcassonne_Desktop.Models.Features;
using Carcassonne_Desktop.Models.Games;
using Carcassonne_Desktop.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Carcassonne_Desktop.ViewModels
{
    public class TileViewerModel : ViewModelBase
    {
        private int angle;
        private List<Feature> flist;
        private List<int> llist;
        private string texture;

        public TileViewerModel()
        {
            InitializeCommand();
        }
        public RelayCommand<String> AddBtnCommand { get; set; }
        public TileViewer Window { get; set; }
        public Game Game { get; set; }

        public string Texture
        {
            get { return texture; }
            set { Set(() => Texture, ref texture, value); }
        }

        public int Angle
        {
            get { return angle; }
            set { Set(() => Angle, ref angle, value); }
        }

        public List<Feature> FeatureList
        {
            get { return flist; }
            set { Set(() => FeatureList, ref flist, value); }
        }

        private Tile SourceTile { get; set; }

        public List<int> LocationList
        {
            get { return llist; }
            set { Set(() => LocationList, ref llist, value); }
        }

        private void InitializeCommand()
        {
            AddBtnCommand = new RelayCommand<String>(SendChoise);
        }

        public void UpdateSettings(Tile source)
        {
            SourceTile = source;
            Texture = Game.NextTile.Texture;
            Angle = Game.NextTile.Angle;
            Game.FindPossiblePawnPositions();
            FeatureList = Game.p_features;
            LocationList = Game.feature_location;
            SetGrid();
            //DEBUG();
        }

        public void SetGrid()
        {
            N = FeatureList[(int) FeaturePosition.north].Type != FeatureType.NONE;
            E = FeatureList[(int) FeaturePosition.east].Type != FeatureType.NONE;
            W = FeatureList[(int) FeaturePosition.west].Type != FeatureType.NONE;
            Z = FeatureList[(int) FeaturePosition.south].Type != FeatureType.NONE;
            C = FeatureList[(int) FeaturePosition.center].Type != FeatureType.NONE;

            Ne = (FeatureList[(int) FeaturePosition.eastNorthEast].Type != FeatureType.NONE
                  && FeatureList[(int) FeaturePosition.northNorthEast].Type != FeatureType.NONE);

            Nw = (FeatureList[(int) FeaturePosition.northNorthWest].Type != FeatureType.NONE
                  && FeatureList[(int) FeaturePosition.westNorthWest].Type != FeatureType.NONE);

            Ze = (FeatureList[(int) FeaturePosition.southSouthEast].Type != FeatureType.NONE
                  && FeatureList[(int) FeaturePosition.eastSouthEast].Type != FeatureType.NONE);

            Zw = (FeatureList[(int) FeaturePosition.southSouthWest].Type != FeatureType.NONE
                  && FeatureList[(int) FeaturePosition.westSouthWest].Type != FeatureType.NONE);
        }

        public void DEBUG()
        {
            Console.WriteLine("***********************************************************");
            Console.WriteLine("***********************************************************");
            Console.WriteLine("**********************DEBUGGING****************************");
            Console.WriteLine("***********************************************************");
            Console.WriteLine("***********************************************************");
            foreach (var f in FeatureList)
                if (f != null)
                    Console.WriteLine("********************TYPE/ID: " + f.Type + " / " + f.ID + " Pos.Count: -na" +
                                      "*************");
                else
                    Console.WriteLine("********************TYPE/ID: null /  Pos.Count: -na" + "*************");
            Console.WriteLine("***********************************************************");
            Console.WriteLine("***********************************************************");
            foreach (var i in LocationList)
                Console.WriteLine("************************i: " + i + "*******************");
        }

        private void SendChoise(String pos)
        {
            var fpos = FeaturePosition.none;
            switch (pos)
            {
                case "N":
                    fpos = FeaturePosition.north;
                    break;
                case "NW":
                    fpos = FeaturePosition.northNorthWest;
                    break;
                case "NE":
                    fpos = FeaturePosition.northNorthEast;
                    break;
                case "E":
                    fpos = FeaturePosition.east;
                    break;
                case "ZE":
                    fpos = FeaturePosition.southSouthEast;
                    break;
                case "ZW":
                    fpos = FeaturePosition.southSouthWest;
                    break;
                case "Z":
                    fpos = FeaturePosition.south;
                    break;
                case "W":
                    fpos = FeaturePosition.west;
                    break;
                case "C":
                    fpos = FeaturePosition.center;
                    break;
                case "none":
                    fpos = FeaturePosition.none;
                    break;
            }
            Game.PlacePawnOnTile(fpos, SourceTile);
            Window.Hide();
            if (Game.isFinished)
            {
                ScoreBoardView scoreView = new ScoreBoardView();
                scoreView.Owner = Window.Owner;
                var sbv = (ScoreBoardModel)scoreView.DataContext;
                sbv.Window = scoreView;
                sbv.Players = Game.Players;
                scoreView.ShowDialog();
            }
        }

        #region Gridpositions

        private bool n, ne, e, ze, z, zw, w, nw, c;

        public bool C
        {
            get { return c; }
            set { Set(() => C, ref c, value); }
        }

        public bool Nw
        {
            get { return nw; }
            set { Set(() => Nw, ref nw, value); }
        }

        public bool W
        {
            get { return w; }
            set { Set(() => W, ref w, value); }
        }

        public bool Zw
        {
            get { return zw; }
            set { Set(() => Zw, ref zw, value); }
        }

        public bool Z
        {
            get { return z; }
            set { Set(() => Z, ref z, value); }
        }

        public bool Ze
        {
            get { return ze; }
            set { Set(() => Ze, ref ze, value); }
        }

        public bool E
        {
            get { return e; }
            set { Set(() => E, ref e, value); }
        }

        public bool Ne
        {
            get { return ne; }
            set { Set(() => Ne, ref ne, value); }
        }

        public bool N
        {
            get { return n; }
            set { Set(() => N, ref n, value); }
        }

        #endregion
    }
}