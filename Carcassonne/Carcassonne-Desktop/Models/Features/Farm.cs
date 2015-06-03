using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Carcassonne_Desktop.Models.Features
{
    public class Farm : Feature
    {
        protected List<City> cities;

        public Farm(int id) : base(id, FeatureType.FARM)
        {
            cities = new List<City>();
        }

        public List<City> Cities
        {
            get { return cities; }
        }

        public override bool Combine(Feature other)
        {
            Debug.WriteLine("***Merging farm");
            if (this == other)
                return true;
            Debug.WriteLine("Combining: " + ToString());
            Debug.WriteLine("\nwith: " + other);
            if (!base.Combine(other))
                return false;

            if (IsSameType(this, other))
            {
                var farm = (Farm) other;
                foreach (var c in farm.cities)
                {
                    Debug.WriteLine("city found in farm: " + c);
                    if (!cities.Contains(c))
                        cities.Add(c);
                }
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var res = base.ToString() +
                      "\nNumber of neigbor cities: " + cities.Count;
            return res;
        }

        public override void OnNewNeighbor(Tile newNeighbor, TileDirection direction)
        {
        }

        public override bool CheckCompleted()
        {
            return false;
        }

        public override int Score()
        {
            return cities.Where(c => c.Completed).Sum(c => 3);
        }
    }
}