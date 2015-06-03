

using System;

namespace Carcassonne_Desktop.Models.NetModels.GameModels
{
    public class Turn
    {
        public Guid GameId { get; set; }
        public int TurnID { get; set; }

        public Player Current { get; set; }

        public int LastTile { get; set; }
        public Location TileLocation { get; set; }
        public int Rotation { get; set; }
        public FeaturePosition PawnLocation { get; set; }

        public Player Next { get; set; }
    }
}
