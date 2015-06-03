using Carcassonne_Web.Models.GameObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carcassonne_Web.DAL
{
    public interface IScoreRepository : IDisposable
    {
        IEnumerable<Scores> GetScoresPerUser();
        IEnumerable<Scores> GetScoresPerUserForMonth(DateTime date);
        IEnumerable<Score> GetScoresForUser(String userId);

        
    }
}