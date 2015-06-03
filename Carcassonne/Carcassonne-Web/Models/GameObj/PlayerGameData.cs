using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Carcassonne_Web.Models.GameObj
{
    public class PlayerGameData
    {
        [Key]
        public int PlayerGameDataID { get; set; }

        public String PlayerID { get; set; }
        public Game Game { get; set; }
        public ApplicationUser Player { get; set; }

        public int Score { get; set; }
        public bool Win { get; set; }
        public int TurnOrder { get; set; }
        public int Pawns { get; set; }

    }
}
