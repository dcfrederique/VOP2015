using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Carcassonne_Desktop.Models.NetModels.GameModels
{
    public class PlayerGameData
    {
        public int PlayerGameDataID { get; set; }

        public String PlayerId { get; set; }
        public Game Game { get; set; }
        public Player Player { get; set; }

        public int Score { get; set; }
        public bool Win { get; set; }
        public int TurnOrder { get; set; }
        public int Pawns { get; set; }

    }
}
