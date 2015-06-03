using Carcassonne_Web.Models;
using Carcassonne_Web.Models.GameObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carcassonne_Web.DAL
{
    public interface IPlayerRepository : IDisposable
    {
        IEnumerable<ApplicationUser> GetPlayers();
        ApplicationUser GetPlayerByID(String PlayerId);
        void InsertPlayer(ApplicationUser Player);
        void DeletePlayer(int PlayerID);
        void UpdatePlayer(ApplicationUser Player);
        void Save();

    }
}