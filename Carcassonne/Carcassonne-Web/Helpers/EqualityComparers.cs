using Carcassonne_Web.Models.GameObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carcassonne_Web.Helpers
{
    public class PlayerEqualityComparer : IEqualityComparer<Player>
    {
        public bool Equals(Player x, Player y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(Player obj)
        {
            return obj.GetHashCode();
        }
    }
}