using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carcassonne_Web.Models.GameObj
{
    public class Scores
    {
        public Player Player { get; set; }

        public int TotalScore { get; set; }

        public int Games { get; set; }

        public int Wins { get; set; }
    }
}