using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carcassonne_Web.Models.GameObj;
using Carcassonne_Web.Models;
using System.Data.Entity;

namespace Carcassonne_Web.DAL
{
    public class PlayerRepository : IPlayerRepository, IDisposable
    {
        private CarcassonneContext context;

        public PlayerRepository(CarcassonneContext context)
        {
            this.context = context;
        }

        public IEnumerable<ApplicationUser> GetPlayers()
        {
            return context.Users.ToList();
        }

        public ApplicationUser GetPlayerByID(String id)
        {
            return  context.Users.Find(id);
        }

        public void InsertPlayer(ApplicationUser Player)
        {
            throw new NotImplementedException();
        }

        public void DeletePlayer(int PlayerID)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayer(ApplicationUser Player)
        {
            throw new NotImplementedException();
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
