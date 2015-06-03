using System;
using System.Diagnostics;

namespace Carcassonne_Desktop.Models.Features
{
    public class Road : Feature
    {
        public Road(int id, int openings) : base(id, FeatureType.ROAD)
        {
            Openings = openings;
        }

        public int Openings { get; set; }

        public override bool Combine(Feature other)
        {
            Debug.WriteLine("\n********************************\n");

            if (parts.Count > 1)
                Debug.WriteLine("-----MORE THAN 1 parts in this feature");

            if (parts.Count == 0)
                Debug.WriteLine("-----ZERO parts in this feature");

            if (this == other)
            {
                Debug.WriteLine("ALERT: This==Other");
                return true;
            }
            Debug.WriteLine("Combining: " + ToString());
            Debug.WriteLine("\nwith: " + other);
            if (!base.Combine(other))
            {
                return false;
            }
            if (IsSameType(this, other))
            {
                var road = (Road) other;
                Openings += road.Openings - 2; // 2 endings just met
                Completed = CheckCompleted();
                Debug.WriteLine("\nResult of merging:" + ToString());
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var res = base.ToString() +
                      "\nNumber of openings: " + Openings;
            if (parts.Count == 1)
                res += "\tTexture: " + parts[0].Texture;

            return res;
        }

        public override void OnNewNeighbor(Tile newNeighbor, TileDirection direction)
        {
        }

        public override bool CheckCompleted()
        {
            Debug.WriteLine("Road openings: " + Openings);
            if (Openings < 0)
            {
                throw new Exception("Error: openings under zero");
            }

            if (Openings == 0)
            {
                Completed = true;
                Debug.WriteLine("********ROAD IS COMPLETED");
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
                return parts.Count;
            }
            return 0;
        }
    }
}