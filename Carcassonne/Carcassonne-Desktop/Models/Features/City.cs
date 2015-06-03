using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Carcassonne_Desktop.Models.Features
{
    public class City : Feature
    {
        protected int flags;
        protected int openings;

        public City(int id) : base(id, FeatureType.CITY)
        {
            openings = 0;
            flags = 0;
        }

        public City(int id, int o, int f) : base(id, FeatureType.CITY)
        {
            openings = o*3;
            flags = f;
        }

        public override void OnNewNeighbor(Tile newNeighbor, TileDirection direction)
        {
        }

        public override bool Combine(Feature other)
        {
            Debug.WriteLine("\n********************************\n");
            if (IsSameType(this, other))
            {
                openings = openings - 2;
                CheckCompleted();
                if (this == other)
                {
                    Debug.WriteLine("ALERT: this==other");
                    return true;
                }
                Debug.WriteLine("Combining: " + ToString());
                Debug.WriteLine("\nwith: " + other);
                if (!base.Combine(other))
                    return false;

                var temp = (City) other;
                flags += temp.flags;
                openings += temp.openings;

                foreach (var tile in parts)
                {
                    foreach (var tileFeature in tile.Features)
                    {
                        if (tileFeature.Type == FeatureType.FARM)
                        {
                            var indexesOfCity = new List<int>();
                            var tempFarm = (Farm) tileFeature;
                            Debug.WriteLine("Farm found: " + tempFarm);
                            foreach (City c in tempFarm.Cities)
                            {
                                if (c == other)
                                {
                                    Debug.WriteLine("City of farm matched with this farm");
                                    indexesOfCity.Add(tempFarm.Cities.IndexOf(c));
                                }
                            }
                            foreach (var index in indexesOfCity)
                            {
                                tempFarm.Cities[index] = this;
                                Debug.WriteLine("index of city for farm: " + index);
                            }
                        }
                    }
                }
                Debug.WriteLine("\nResult of merging:" + ToString());
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var res = base.ToString() +
                      "\nNumber of openings: " + openings +
                      "\tNumber of flags: " + flags;
            return res;
        }

        public override bool CheckCompleted()
        {
            Debug.WriteLine("City Openings: " + openings);
            if (openings < 0)
                throw new Exception("Error, less then zero");

            if (openings == 0)
            {
                Debug.WriteLine("********CITY IS COMPLETED");
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
                scored = false;
                if (completed)
                {
                    if (parts.Count > 2)
                        return (parts.Count + flags)*2;
                    return (parts.Count + flags);
                }
                return (parts.Count + flags);
            }
            return 0;
        }
    }
}