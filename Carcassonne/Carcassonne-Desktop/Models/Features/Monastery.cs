using System.Diagnostics;

namespace Carcassonne_Desktop.Models.Features
{
   public class Monastery : Feature
    {
        public const uint MAX_NEIGHBORS = 8;
        protected int neighbors;

        public Monastery(int id) : base(id, FeatureType.MONASTERY)
        {
            neighbors = 0;
        }

        public Monastery(int id, int n) : base(id, FeatureType.MONASTERY)
        {
            neighbors = n;
        }

        public override bool Combine(Feature old)
        {
            return false;
        }

        public override string ToString()
        {
            var res = base.ToString() +
                      "\nNumber of neigbors: " + neighbors;
            return res;
        }

        public override void OnNewNeighbor(Tile newNeighbor, TileDirection direction)
        {
            neighbors++;
            CheckCompleted();
        }

        public override bool CheckCompleted()
        {
            Debug.WriteLine("Monastery has " + neighbors + " neighbors.");
            if (neighbors == MAX_NEIGHBORS)
            {
                Debug.WriteLine("Monastry is completed");
                Completed = true;
                return Completed;
            }
            Completed = false;
            return Completed;
        }

        public override int Score()
        {
            if (!scored)
            {
                scored = true;
                return (neighbors + 1);
            }
            return 0;
        }
    }
}