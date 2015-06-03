using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carcassonne_Web.Models.GameObj
{
    public class Score
    {
        public Player Player { get; set; }

        public Game Game { get; set; }

        public int AchievedScore { get; set; }

        public bool Win { get; set; }

    }
}