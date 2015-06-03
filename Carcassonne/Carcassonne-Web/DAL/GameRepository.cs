using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carcassonne_Web.Models.GameObj;
using Carcassonne_Web.Models;
using System.Data.Entity;

namespace Carcassonne_Web.DAL
{
    public class GameRepository : IGameRepository, IDisposable
    {
        private CarcassonneContext context;

        public GameRepository(CarcassonneContext context)
        {
            this.context = context;
        }

        public IEnumerable<Game> GetGames()
        {
            return context.Games.ToList();
        }

        public Game GetGameByID(Guid id)
        {
            var Game = context.Games
                .Include(x => x.PlayerData)
                .Include(x => x.PlayerData.Select(y => y.Player))
                .Where(x => x.GameID == id).FirstOrDefault();
            return Game;
        }

        public void InsertGame(Game Game)
        {
            context.Games.Add(Game);
        }

        public void DeleteGame(Guid GameID)
        {
            Game Game = context.Games.Find(GameID);
            context.Games.Remove(Game);
        }

        public void UpdateGame(Game Game)
        {
            context.Entry(Game).State = EntityState.Modified;
        }

        public void AddTurn(Turn turn, Game game)
        {
            Turn t = new Turn(turn);
            t.CurrentPlayer = context.Users.Find(t.Current.ID);

            if (game.Turns == null)
            {
                game.Turns = new List<Turn>();
            }
            game.Turns.Add(t);
            UpdateGame(game);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
