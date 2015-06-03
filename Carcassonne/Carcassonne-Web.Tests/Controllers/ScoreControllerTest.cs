using System;
using System.Collections.Generic;
using System.Linq;
using Carcassonne_Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carcassonne_Web.Models.GameObj;

namespace Carcassonne_Web.Tests.Controllers
{
    [TestClass]
    public class ScoreControllerTest
    {
        [TestMethod]
        public void TestScoresForUser()
        {
            ScoresController Scores = new ScoresController();

            IEnumerable<Score> score = Scores.Get("00000000");

            if (score.Any())
            {
                Assert.Fail("Shows scores for non-existing user");
            }

        }
    }
}
