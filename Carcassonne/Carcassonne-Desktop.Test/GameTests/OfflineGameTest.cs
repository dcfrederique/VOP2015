using System.Linq;
using Carcassonne_Desktop.Models;
using Carcassonne_Desktop.Models.Games;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carcassonne_Desktop.Test.GameTests
{
    [TestClass]
    public class OfflineGameTest
    {
        [TestMethod]
        public void InitialTileCountTest()
        {
            Game offlineGame = new OfflineGame();
            //71 => 72 tegels - starttegel
            Assert.AreEqual(offlineGame.Tiles.Count, 71);
        }

        [TestMethod]
        public void PullTileTest()
        {
            Game offlineGame = new OfflineGame();
            offlineGame.PullTile();
            Assert.IsInstanceOfType(offlineGame.NextTile, typeof (Tile));
        }

        //TEST Initialize method(amount of cpu players, correct username, correct avatar
        [TestMethod]
        public void InitializeTest()
        {
            var offlineGame = new OfflineGame();
            offlineGame.Initialize("test", 3, "..\\resources\\Pawns\\pawn_blu.png");
            Assert.IsFalse(offlineGame.CurPlayer.IsCpuPlayer);
            Assert.AreEqual(offlineGame.CurPlayer.Username,"test");
            Assert.AreEqual(offlineGame.Players.Count,4);
            Assert.AreEqual(offlineGame.Players.Count(p => p.IsCpuPlayer),3);
            Assert.AreEqual(offlineGame.CurPlayer.Avatar, "..\\resources\\Pawns\\pawn_blu.png");

        }

    }
}