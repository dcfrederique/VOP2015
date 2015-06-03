using System;
namespace Carcassonne_Desktop.Models.Features
{
    public class None: Feature
    {
        public None():base(0, FeatureType.NONE)
        {

        }
        public override bool Combine(Feature old)
        {
            return false;
        }
        public override void OnNewNeighbor(Tile newNeighbor, TileDirection direction)
        {
            throw new NotImplementedException();
        }
        public override bool CheckCompleted()
        {
            throw new NotImplementedException();
        }
        public override int Score()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "zerofeature";
        }
    }
}
