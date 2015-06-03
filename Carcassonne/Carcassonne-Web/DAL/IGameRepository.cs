using Carcassonne_Web.Models.GameObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carcassonne_Web.DAL
{
    public interface IGameRepository : IDisposable
    {
        IEnumerable<Game> GetGames();
        Game GetGameByID(Guid GameId);
        void InsertGame(Game Game);
        void DeleteGame(Guid GameID);
        void UpdateGame(Game Game);
        void Save();

        void AddTurn(Turn turn, Game game);
    }
}