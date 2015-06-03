namespace Carcassonne_Desktop.Models
{
    public class Pawn
    {
        public Pawn(Player pid)
        {
            Player = pid;
            Texture = pid.Avatar;
            Type = FeatureType.NONE;
        }

        public Pawn()
        {
        }

        public Player Player { get; set; }
        public FeatureType Type { get; set; }
        public string Texture { get; set; }

        public void remove_pawn()
        {
            Type = FeatureType.NONE;
        }
    }
}