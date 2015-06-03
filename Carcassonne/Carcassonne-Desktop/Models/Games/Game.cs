using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carcassonne_Desktop.Models.Data;
using Carcassonne_Desktop.Models.Etc;
using Carcassonne_Desktop.Models.Features;
using Carcassonne_Desktop.Models.NetModels.GameModels;
using GalaSoft.MvvmLight;
namespace Carcassonne_Desktop.Models.Games
{
    public abstract class Game : ObservableObject
    {
        #region VARIABLES

        public Board Board { get; set; }
        public List<Tile> Tiles { get; set; }
        protected Tile nextTile;
        protected Player currentPlayer;
        protected Player thisPlayer;
        protected List<Player> players;
        protected Random random;
        public List<Feature> currentFeatures;
        public List<Feature> p_features;
        public List<int> feature_location;
        public bool isFinished;
        private bool canPlaceOnBoard;
        public Turn currentTurn;
        public Logging logger;

        #endregion
        #region PROPERTIES

        /// <summary>
        ///     Stores the next tile to be play with
        /// </summary>
        public Tile NextTile
        {
            get { return nextTile; }
            set { Set(() => NextTile, ref nextTile, value); }
        }

        public bool CanPlaceOnBoard
        {
            get { return canPlaceOnBoard;}
            set { Set(() => CanPlaceOnBoard, ref canPlaceOnBoard, value); }
        }

        /// <summary>
        ///     Stores the current player
        /// </summary>
        public Player ThisPlayer
        {
            get { return thisPlayer; }
            set { Set(() => ThisPlayer, ref thisPlayer, value); }
        }

        public Player CurPlayer
        {
            get { return currentPlayer; }
            set { Set(() => CurPlayer, ref currentPlayer, value); }
        }

        public List<Player> Players
        {
            get { return players; }
            set { Set(() => Players, ref players, value); }
        }

        #endregion PROPERTIES

        protected Game()
        {
            Board = new Board();
            logger = new Logging();
            currentFeatures = new List<Feature>();
            random = new Random();
            canPlaceOnBoard = true;
        }
        public abstract void PlacePawnOnTile(FeaturePosition pos, Tile source);
        public abstract void EndGame();
        public abstract void PlaceTileOnBoard(Tile t);

        public void PullTile()
        {
            int tilescount = Tiles.Count;
            if (tilescount > 0)
            {
                int id = random.Next(tilescount - 1);
                NextTile = Tiles.ElementAt(id);
                Tiles.RemoveAt(id);
                logger.Log("Pulling new tile. Tilestack has now " + Tiles.Count + " tiles left.");
            }
            else
            {
                    logger.Log("End of Game");
                    EndGame();
            }
        }

        public void LoadTilesFromXml(String url, bool shuffle)
        {
            List<Tile> temp = TileLoader.getInstance.LoadTiles(new StringReader(url));
            Tile startTile = temp[temp.Count - 1];
            startTile.Location = new Location(0, 0);
            startTile.Enabled = true;
            startTile.Visibility = true;
            Board.Tiles.Add(startTile);
            foreach (Feature f in startTile.Features.Where(f => !currentFeatures.Contains(f)))
            {
                currentFeatures.Add(f);
            }
            temp.RemoveAt(temp.Count - 1);
            if (shuffle) temp.Shuffle();
            Tiles = temp;
            logger.Log("Loaded "+Tiles.Count+" tiles from xml");
            MakeEmptyTilesAroundTile(startTile);
        }

        public void MakeEmptyTilesAroundTile(Tile t)
        {
            Board.MakePossibleLeftTile(t);
            Board.MakePossibleRightTile(t);
            Board.MakePossibleUpTile(t);
            Board.MakePossibleBottomTile(t);          
        }
        public bool IsTileValid(Tile tile)
        {
            var count = 0;
            tile.Rotate_left();
            count += FindAmountOfValidPlaces(tile);
            tile.Rotate_left();
            count += FindAmountOfValidPlaces(tile);
            tile.Rotate_left();
            count += FindAmountOfValidPlaces(tile);
            tile.Rotate_left();
            FindAmountOfValidPlaces(tile);
            return count != 0;

        }
        public void OrderPlayersByScore()
        {
            players = players.OrderByDescending(t => t.Score).ToList();
            for(int i = 0 ;i<players.Count;i++)
            {
                logger.Log("Rank " +i+"- "+players[i].Username + " has score : " + players[i].Score );
            }
        }
        public int FindAmountOfValidPlaces(Tile tile)
        {
            var count = 0;
            foreach (var t in Board.Tiles.Where(t => t.Texture == null))
            {
                t.Visibility = true;
                if (Board.IsPlaceValid(t, tile))
                    count++;
            }
            logger.Log("Found "+ count + " valid places");
            return count;
            
        }
        public void MergeFeatures()
        {
            var x = nextTile.Location.X;
            var y = nextTile.Location.Y;

            try
            {
                if ( /*y != 0 &&*/ Board.IsTileSet(x, y - 1))
                {
                    nextTile.CombineFeatures(TileDirection.north, Board.GetTile(x, y - 1));
                }
                if (Board.IsTileSet(x + 1, y))
                {
                    nextTile.CombineFeatures(TileDirection.east, Board.GetTile(x + 1, y));
                }
                if (Board.IsTileSet(x, y + 1))
                {
                    nextTile.CombineFeatures(TileDirection.south, Board.GetTile(x, y + 1));
                }
                if ( /*x!= && */Board.IsTileSet(x - 1, y))
                {
                    nextTile.CombineFeatures(TileDirection.west, Board.GetTile(x - 1, y));
                }
            }
            catch (NullReferenceException e)
            {
            }

            if (Board.IsTileSet(x - 1, y - 1))
            {
                nextTile.OnNewNeighbor(Board.GetTile(x - 1, y - 1), TileDirection.northWest);
                Board.GetTile(x - 1, y - 1).OnNewNeighbor(nextTile, TileDirection.southEast);
            }
            if (Board.IsTileSet(x, y - 1))
            {
                nextTile.OnNewNeighbor(Board.GetTile(x, y - 1), TileDirection.north);
                Board.GetTile(x, y - 1).OnNewNeighbor(nextTile, TileDirection.south);
            }
            if (Board.IsTileSet(x + 1, y - 1))
            {
                nextTile.OnNewNeighbor(Board.GetTile(x + 1, y - 1), TileDirection.northEast);
                Board.GetTile(x + 1, y - 1).OnNewNeighbor(nextTile, TileDirection.southWest);
            }
            if (Board.IsTileSet(x + 1, y))
            {
                nextTile.OnNewNeighbor(Board.GetTile(x + 1, y), TileDirection.east);
                Board.GetTile(x + 1, y).OnNewNeighbor(nextTile, TileDirection.west);
            }
            if (Board.IsTileSet(x + 1, y + 1))
            {
                nextTile.OnNewNeighbor(Board.GetTile(x + 1, y + 1), TileDirection.southEast);
                Board.GetTile(x + 1, y + 1).OnNewNeighbor(nextTile, TileDirection.northWest);
            }
            if (Board.IsTileSet(x, y + 1))
            {
                nextTile.OnNewNeighbor(Board.GetTile(x, y + 1), TileDirection.south);
                Board.GetTile(x, y + 1).OnNewNeighbor(nextTile, TileDirection.north);
            }
            if (Board.IsTileSet(x - 1, y + 1))
            {
                nextTile.OnNewNeighbor(Board.GetTile(x - 1, y + 1), TileDirection.southWest);
                Board.GetTile(x - 1, y + 1).OnNewNeighbor(nextTile, TileDirection.northEast);
            }
            if (Board.IsTileSet(x - 1, y))
            {
                nextTile.OnNewNeighbor(Board.GetTile(x - 1, y), TileDirection.west);
                Board.GetTile(x - 1, y).OnNewNeighbor(nextTile, TileDirection.east);
            }
        }
       
        public void FindPossiblePawnPositions()
        {
            p_features = new List<Feature>();
            feature_location = new List<int>();
            Feature nullf = new None();
            var roads = new List<Road>();

            for (var i = 0; i < nextTile.Features.Count; i++)
            {
                if (nextTile.Features[i].Type == FeatureType.ROAD)
                {
                    roads.Add((Road)nextTile.Features[i]);
                }

                if (nextTile.Features[i].CalculateClaim().Count == 0)
                {
                    p_features.Add(nextTile.Features[i]);
                    feature_location.Add(i);
                }
                else
                {
                    p_features.Add(nullf);
                    feature_location.Add(i);
                }
            }

            if (roads.Count > 3)
                p_features[(int)FeaturePosition.center] = nullf;

            
        }
        public void CalculateScore(bool endGame)
        {
            var removalList = new List<Feature>();
            foreach (var f in currentFeatures)
            {
                if (f.HasTiles())
                {
                    List<Player> claims = f.CalculateClaim();
                    if (!f.Scored && (f.Completed || endGame))
                    {
                        var score = 0;
                        if ((score = f.Score()) != 0)
                        {
                            foreach (var p in claims)
                                p.Score += score;
                            f.ReturnPawn(nextTile);
                        }
                    }
                }
                else
                {
                    removalList.Add(f);
                }
            }

            foreach (var f in removalList)
                currentFeatures.Remove(f);
            removalList.Clear();

        }

    }
}