using Carcassonne_Web.DAL;
using Carcassonne_Web.Helpers;
using Carcassonne_Web.Models.GameObj;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Carcassonne_Web.Controllers
{
    [RoutePrefix("api/Scores")]
    [Authorize]
    public class ScoresController : ApiController
    {
        private IScoreRepository scoreRepo { get; set; }

        public ScoresController()
        {
            scoreRepo = new ScoreRepository(new CarcassonneContext());
        }

        // GET: api/Scores
        public IEnumerable<Scores> Get()
        {
           return scoreRepo.GetScoresPerUser();
        }

        // GET: api/Scores/month
        [Route("month/{date:long}")]
        public IEnumerable<Scores> GetScoresPerUserForMonth(long date = 0)
        {
            if (date != 0)
            {
                DateTime dt = new DateTime(1970, 1, 1).AddTicks(date * 10000);

                return scoreRepo.GetScoresPerUserForMonth(dt);
            }
            else
            {
                return scoreRepo.GetScoresPerUserForMonth(DateTime.Now);
            }
        }

        // GET: api/Scores/userId
        public IEnumerable<Score> Get(string id)
        {
            return scoreRepo.GetScoresForUser(id);

        }

        // GET: api/Scores/total/userId
        [Route("{id}/total")]
        public object GetTotalScore(string id)
        {
            var res = scoreRepo.GetScoresForUser(id);

            var totalres = from x in res
                           select new
                           {
                               score = res.Select(y => y.AchievedScore).Sum().ToString(),
                               wins = res.Where(y => y.Win).Count().ToString(),
                               total = res.Count().ToString(),
                           };

            return totalres.FirstOrDefault();

        }

    }
}
