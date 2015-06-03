using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Carcassonne_Web.Models.GameObj
{
    public class Game
    {
        [Key]
        public Guid GameID { get; set; }
        public DateTime Started { get; set; }
        public DateTime LastTurn { get; set; }

        public ICollection<PlayerGameData> PlayerData { get; set; }
        public ICollection<Turn> Turns { get; set; }
    }
}
