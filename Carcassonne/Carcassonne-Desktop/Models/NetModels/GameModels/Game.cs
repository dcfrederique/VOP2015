using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Carcassonne_Desktop.Models.NetModels.GameModels
{
    public class Game
    {
        public Guid GameID { get; set; }
        public DateTime Started { get; set; }
        public DateTime LastTurn { get; set; }

        public ICollection<PlayerGameData> PlayerData { get; set; }
        public ICollection<Turn> Turns { get; set; }
    }
}
