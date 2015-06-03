using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Carcassonne_Desktop.Models.Etc;
using Carcassonne_Desktop.Models.NetModels.GameModels;
using Carcassonne_Desktop.Properties;

namespace Carcassonne_Desktop.Models.Games
{
    public class OfflineGame : Game
    {
        public OfflineGame()
        {
            LoadTilesFromXml(Resources._10Tiles, true);
        }

        public void Initialize(string playerName, int amountOfCpuPlayers, string chosenAvatar)
        {
            var resources = new List<string>
            {
                "..\\resources\\Pawns\\pawn_blu.png",
                "..\\resources\\Pawns\\pawn_blk.png",
                "..\\resources\\Pawns\\pawn_grn.png",
                "..\\resources\\Pawns\\pawn_red.png",
                "..\\resources\\Pawns\\pawn_ylw.png"
            };
            Players = new List<Player>();
            var temp = new Player(playerName) {Avatar = chosenAvatar, IsCpuPlayer = false};
            thisPlayer = temp;
            Players.Add(temp);
            resources.Remove(chosenAvatar);
            resources.Shuffle();
            for (var i = 0; i < amountOfCpuPlayers; i++)
            {
                var t = new Player("cpu " + (i + 1)) {Avatar = resources[i], IsCpuPlayer = true};
                Players.Add(t);
            }
            CurPlayer = Players.First();
            currentTurn = new Turn();
            PullTile();
            FindAmountOfValidPlaces(NextTile);
        }

        public override void PlacePawnOnTile(FeaturePosition pos, Tile source)
        {
            if (pos != FeaturePosition.none)
            {
                var pawn = currentPlayer.get_pawn();
                pawn.Player = currentPlayer;
                pawn.Type = p_features[(int) pos].Type;
                p_features[(int) pos].AddPawn(pawn);
                NextTile.Place_pawn(pos, pawn);
            }
            p_features.Clear();
            feature_location.Clear();
            PlaceTileOnBoard(NextTile);
            CalculateScore(false);
            ProcessEndTurn(source);
        }

        private void ProcessEndTurn(Tile t)
        {
            ChangePlayer();
            PullTile();
            if (!isFinished)
            {
            
                if (t != null)
                {
                    MakeEmptyTilesAroundTile(t);
                }
                while (FindAmountOfValidPlaces(NextTile) == 0 && !IsTileValid(NextTile))
                {
                    Debug.WriteLine("Found card with no possible options. Put back behind deck");
                    Tiles.Add(NextTile);
                    PullTile();
                }
                if (currentPlayer.IsCpuPlayer)
                {
                    CanPlaceOnBoard = false;
                    MakeCpuMove();
                }
                else
                {
                    CanPlaceOnBoard = true;
                }
            }
        }

        private async Task PutTaskDelay()
        {
            await Task.Delay(1000);
        }

        private async void MakeCpuMove()
        {
            var emptyTiles = Board.Tiles.Where(t => t.Visibility && t.Texture == null).ToList();
            while (emptyTiles.Count == 0)
            {
                NextTile.Rotate_left();
                NextTile.Angle -= 90;
                FindAmountOfValidPlaces(NextTile);
                emptyTiles = Board.Tiles.Where(t => t.Visibility && t.Texture == null).ToList();
            }
            var chosenLocation = emptyTiles.GetRandomElement();
            NextTile.Location = chosenLocation.Location;
            MergeFeatures();
            FindPossiblePawnPositions();
            FeaturePosition fpos;
            var rand = new Random();
            var chance = 4; // 25% kans op plaatsen van pion
            if (p_features.Where(t => t.Type == FeatureType.NONE).ToList().Count == p_features.Count ||
                rand.Next(chance) != 0)
            {
                fpos = FeaturePosition.none;
            }
            else
            {
                var f = p_features.GetRandomElement();
                while (f.Type == FeatureType.NONE)
                {
                    f = p_features.GetRandomElement();
                }
                fpos = (FeaturePosition) p_features.IndexOf(f);
            }
            await PutTaskDelay();
            chosenLocation.MarkSelected();
            await PutTaskDelay();
            PlacePawnOnTile(fpos, chosenLocation);
        }

        private void ChangePlayer()
        {
            CurPlayer = Players[(Players.IndexOf(currentPlayer) + 1)%Players.Count];
            logger.Log("Player changed - " + CurPlayer.Username + " turn");
        }

        public override void EndGame()
        {
            isFinished = true;
            CanPlaceOnBoard = false;
            NextTile = null;
            CalculateScore(true);
            OrderPlayersByScore();
            RaisePropertyChanged("isFinished");
        }

        public override void PlaceTileOnBoard(Tile t)
        {
            foreach (var f in nextTile.Features.Where(f => !currentFeatures.Contains(f)))
            {
                currentFeatures.Add(f);
            }
            NextTile.Visibility = true;
            Board.AddTile(NextTile);
            logger.Log("Tile placed on board - Tile location: " + "(" + NextTile.Location.X + "," + NextTile.Location.Y +
                       ")");
        }
    }
}