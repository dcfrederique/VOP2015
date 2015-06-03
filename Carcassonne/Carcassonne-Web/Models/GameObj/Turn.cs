using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carcassonne_Desktop.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Carcassonne_Web.Models.GameObj
{
    public class Turn
    {
        public Turn() { }

        public Turn(Turn turn)
        {
            GameId = turn.GameId;
            TurnID = turn.TurnID;

            LastTile = turn.LastTile;
            TileLocation = turn.TileLocation;
            Rotation = turn.Rotation;
            PawnLocation = turn.PawnLocation;

            Current = turn.Current;
            CurrentPlayer = turn.CurrentPlayer;
            Next = turn.Next;
            NextPlayer = turn.NextPlayer;
        }

        [Key]
        [JsonIgnore]
        public Game Game { get; set; }

        [NotMapped]
        public Guid GameId { get; set; }

        [Key]
        public int TurnID { get; set; }

        [NotMapped]
        public Player Current { get; set; }
        [JsonIgnore]
        public ApplicationUser CurrentPlayer { get; set; }

        public int LastTile { get; set; }
        public Location TileLocation { get; set; }
        public TileDirection Rotation { get; set; }
        public FeaturePosition PawnLocation { get; set; }

        [NotMapped]
        public Player Next { get; set; }
        [JsonIgnore]
        public ApplicationUser NextPlayer { get; set; }
    }
}
