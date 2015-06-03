using System.Collections.Generic;
using System.Linq;
using Carcassonne_Desktop.Models.Games;
using Carcassonne_Desktop.Models.Etc;
using System;
using System.Collections;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using Carcassonne_Desktop.Models.NetModels;
using Carcassonne_Desktop.Models.NetModels.GameModels;
using System.Windows;

namespace Carcassonne_Desktop.Models.Games
{
    public class WebGame : Game
    {
        private WebGameConnection serverConn;

        public WebGame(string gameid)
        {
            LoadTilesFromXml(Properties.Resources.tileData, false);
            Debug.WriteLine("STARTED ONLINE GAME WITH ID: " + gameid);
            serverConn = new WebGameConnection();
            GetGameDataFromServer(gameid);
        }

        public void Initialize(ICollection<PlayerGameData> playerDatas)
        {
            List<String> resources = new List<string>     
            {
                "..\\resources\\Pawns\\pawn_blu.png",
                "..\\resources\\Pawns\\pawn_blk.png",
                "..\\resources\\Pawns\\pawn_grn.png",
                "..\\resources\\Pawns\\pawn_red.png",
                "..\\resources\\Pawns\\pawn_ylw.png"
            };
            
            var players = new List<Player>();
            foreach (var a in playerDatas)
            {
                Player p = a.Player;
                p.GameData = a;
                p.IsCpuPlayer = false;
                if (p.Avatar == null)
                {
                    p.Avatar = resources[random.Next(5)];
                }
                players.Add(p);
            }
            Players = players;
            currentTurn = new Turn();
            CurPlayer = Players.First<Player>();
            //string userid = UserState._getInstance().GetUser().Id;
        }

        private void SetThisPlayer(string id)
        {
            ThisPlayer = Players.First(f => f.ID == id);
            logger.Log("You are set as player: "+ ThisPlayer.Username);
            IsMyTurn();
        }

        private void GetGameDataFromServer(string id)
        {
            serverConn.Opened += startedConnection;
            serverConn.ReceivedTurn += GetTurn;
            serverConn.Connect(new Guid(id));
        }

        private void startedConnection(string obj)
        {
            Initialize(serverConn.Game.PlayerData);
            currentTurn.GameId = serverConn.Game.GameID;
            logger.Log("Received game information from server. Game ID: " + currentTurn.GameId);
            SetThisPlayer(UserState._getInstance().GetUser().Id);
        }

        public bool IsMyTurn()
        {
            if (CurPlayer == ThisPlayer)
            {
                logger.Log("***It's my turn to play***");
                CanPlaceOnBoard = true;
                PullTile();
                if (FindAmountOfValidPlaces(NextTile) == 0 && !IsTileValid(NextTile))
                {
                    Debug.WriteLine("Found card with no possible options. Put back behind deck");
                    Tiles.Add(NextTile);
                    PullTile();
                }
                return true;
            }
            else
            {
                logger.Log("***It's "+ CurPlayer.Username + " turn to play***");
                NextTile = null;
                CanPlaceOnBoard = false;
                return false;
            }
        }


        public void GetTurn(NetModels.GameModels.Turn t)
        {
            //Receive the turn from other player
            logger.Log("Receiving turn of player: "+ CurPlayer.Username);
            NextTile = Tiles.FirstOrDefault(f => f.TileID == t.LastTile);
            NextTile.Location = t.TileLocation;
            Tile tmp = new Tile(t.TileLocation);
            Application.Current.Dispatcher.Invoke(
                new Action(() => MakeEmptyTilesAroundTile(tmp)));
            int angle = t.Rotation%360;
            NextTile.Angle = angle;
            switch (angle)
            {
                case 90:
                case -270:
                    NextTile.Rotate_right();
                    break;
                case 180:
                case -180:
                    NextTile.Rotate_right();
                    NextTile.Rotate_right();
                    break;
                case 270:
                case -90:
                    NextTile.Rotate_left();
                    break;
            }
            Application.Current.Dispatcher.Invoke(
                new Action(() =>  FindAmountOfValidPlaces(NextTile)));
            Tiles.Remove(NextTile);
            FeaturePosition pawnPos = t.PawnLocation;
            MergeFeatures();
            FindPossiblePawnPositions();
            Application.Current.Dispatcher.Invoke(
                new Action(() => PlacePawnOnTile(pawnPos, tmp)));
        }

        public void SendTurn()
        {
            logger.Log("Sending my turn to the server");
            currentTurn.Current = CurPlayer;
            currentTurn.LastTile = NextTile.TileID;
            currentTurn.TileLocation = NextTile.Location;
            currentTurn.PawnLocation = NextTile.PawnPosition;
            currentTurn.Rotation = NextTile.Angle;
            serverConn.SendTurn(currentTurn);
        }

        public void Leave()
        {
            logger.Log("I'm leaving!");
            serverConn.Close();
            foreach (Pawn p in CurPlayer.Pawns)
            {
                p.remove_pawn();
            }
        }

        public override void PlacePawnOnTile(FeaturePosition pos, Tile source)
        {
            if (pos != FeaturePosition.none)
            {
                var pawn = currentPlayer.get_pawn();
                pawn.Player = currentPlayer;
                pawn.Type = p_features[(int)pos].Type;
                p_features[(int)pos].AddPawn(pawn);
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
            if (!isFinished)
            {
                if (ThisPlayer == CurPlayer)
                {
                    SendTurn();
                }
                ChangePlayer(null);
                if (t != null)
                    MakeEmptyTilesAroundTile(t);
            }
        }


        private void ChangePlayer(Player p)
        {
            //CurPlayer = p;
            CurPlayer = Players[(Players.IndexOf(currentPlayer) + 1) % Players.Count];
            IsMyTurn();
        }

        public override void EndGame()
        {
            foreach (var player in Players )
            {
                PlayerGameData data = serverConn.Game.PlayerData.First(f => f.Player == player);
                data.Score = ThisPlayer.Score;
                data.PlayerId = ThisPlayer.ID;
            }
            serverConn.SendScores(serverConn.Game);
            serverConn.Close();

            isFinished = true;
            CanPlaceOnBoard = false;
            NextTile = null;
            CalculateScore(true);
            OrderPlayersByScore();
        }

        public override void PlaceTileOnBoard(Tile t)
        {
            foreach (var f in nextTile.Features.Where(f => !currentFeatures.Contains(f)))
            {
                currentFeatures.Add(f);
            }
            NextTile.Visibility = true;
            Board.AddTile(NextTile);
        }
    }
}