using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne_Desktop.Models
{
    public enum FeaturePosition
    {
        //Posities van de tegel per kant
        northNorthWest = 0, north = 1, northNorthEast = 2,
        eastNorthEast = 3, east = 4, eastSouthEast = 5,
        southSouthEast = 6, south = 7, southSouthWest = 8,
        westSouthWest = 9, west = 10, westNorthWest = 11,
        center = 12,
        none = -1
    }
    public enum TileDirection  
    {
        northWest = 0, north = 1, northEast = 2,
        east = 3, southEast = 4, south = 5,
        southWest = 6, west = 7
    }
}
