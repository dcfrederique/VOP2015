using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carcassonne_Web.Models.GameObj;
using Carcassonne_Web.Models;
using System.Data.Entity;

namespace Carcassonne_Web.DAL
{
    public class ScoreRepository : IScoreRepository, IDisposable
    {
        private CarcassonneContext context;

        public ScoreRepository(CarcassonneContext context)
        {
            this.context = context;
        }

        public IEnumerable<Scores> GetScoresPerUser()
        {
            //ICollection<Scores> scores = new List<Scores>();

            var results =
                from r in context.PlayerData.Include(x => x.Game).Include(x => x.Player)
                group r by r.Player into g
                select new Scores()
                {
                    Games = g.Count(),
                    Player = new Player()
                    {
                        ID = g.Key.Id,
                        Avatar = g.Key.Avatar,
                        UserName = g.Key.UserName
                    },
                    TotalScore = g.Sum(x => x.Score),
                    Wins = g.Count(x => x.Win)
                };

            //TODO: evt vervangen door stored procedure, of gewoon door query, en analoog voor elke gebruiker

            return results;

        }

        public IEnumerable<Scores> GetScoresPerUserForMonth(DateTime date)
        {
            var results =
                    from r in context.PlayerData.Include(x => x.Game).Include(x => x.Player)
                    where r.Game.Started.Month == date.Month && r.Game.Started.Year == date.Year
                    group r by r.Player into g
                    select new Scores()
                    {
                        Games = g.Count(),
                        Player = new Player()
                        {
                            ID = g.Key.Id,
                            Avatar = g.Key.Avatar,
                            UserName = g.Key.UserName
                        },
                        TotalScore = g.Sum(x => x.Score),
                        Wins = g.Count(x => x.Win)
                    };

            return results;
        }

        public IEnumerable<Score> GetScoresForUser(string userId)
        {
            var res = context.PlayerData.Include(x => x.Game).Include(x => x.Player).Where(x => x.Player.Id == userId);

            ICollection<Score> scores = new List<Score>();

            foreach (var item in res)
            {
                scores.Add(new Score()
                {
                    Game = item.Game,
                    Player = null,
                    Win = item.Win,
                    AchievedScore = item.Score
                });
            }

            return scores;
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
