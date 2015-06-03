using System;
using System.Collections.Generic;

namespace Carcassonne_Desktop.Models.Features
{
    public abstract class Feature
    {
        protected List<Pawn> claims;
        protected bool completed;
        protected int id; //Unique ID for features
        protected List<Tile> parts;
        protected bool scored;
        protected FeatureType type;

        protected Feature(int id, FeatureType type)
        {
            this.id = id;
            this.type = type;
            scored = false;
            completed = false;
            claims = new List<Pawn>();
            parts = new List<Tile>();
        }

        public Feature(Feature copy)
        {
            id = copy.id;
            type = copy.type;
            scored = copy.scored;
            completed = copy.completed;
            claims = new List<Pawn>();
            parts = new List<Tile>();

            foreach (var pawn in copy.claims)
                claims.Add(pawn);
            foreach (var tile in copy.parts)
                parts.Add(tile);
        }

        public FeatureType Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool Scored
        {
            get { return scored; }
            set { scored = value; }
        }

        public bool Completed
        {
            get { return completed; }
            set { completed = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        internal List<FeaturePosition> Positions { get; set; }

        public bool HasTiles()
        {
            if (parts.Count > 0)
                return true;
            return false;
        }

        public void ReturnPawn(Tile current)
        {
            foreach (var t in parts)
            {
                if (t.PawnTexture != null && t.Features[(int) t.PawnPosition] == this)
                {
                    t.PawnTexture = null;
                    t.PawnX = 0;
                    t.PawnY = 0;   
                }
            }
            foreach (var p in claims)
            {
                p.Player.add_pawn(p);
            }
            claims.Clear();
        }

        public virtual void AddTile(Tile t)
        {
            parts.Add(t);
        }

        public virtual void AddPawn(Pawn p)
        {
            claims.Add(p);
        }

        public abstract void OnNewNeighbor(Tile newNeighbor, TileDirection direction);
        public abstract bool CheckCompleted();
        public abstract int Score();

        public virtual bool Combine(Feature old)
        {
            if (IsSameType(this, old))
            {
                foreach (var tile in old.parts)
                {
                    parts.Add(tile);
                    tile.OnCombinedFeature(old, this);
                }
                old.parts.Clear();

                foreach (var pawn in old.claims)
                {
                    claims.Add(pawn);
                }
                old.claims.Clear();

                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var res =
                "Feature type: " + Type + " " + ID +
                "\tNumber of parts: " + parts.Count +
                "\tNumber of claims: " + claims.Count;
            return res;
        }

        public static bool IsSameType(Feature a, Feature b)
        {
            if (a.type == b.type)
                return true;
            return false;
        }

        public virtual List<Player> CalculateClaim()
        {
            // Make a list of each player and the number of claims the have based upon the men attached to this feature
            var claims = new List<Player>();
            var numClaims = new List<int>();
            foreach (var claim in this.claims)
            {
                var index = -1;
                if ((index = claims.IndexOf(claim.Player)) >= 0)
                {
                    numClaims[index]++;
                }
                else
                {
                    claims.Add(claim.Player);
                    numClaims.Add(1);
                }
            }

            var claimed = new List<Player>();
            var max = 0;
            for (var index = 0; index < numClaims.Count; index++)
            {
                if (numClaims[index] > max)
                {
                    claimed.Clear();
                    claimed.Add(claims[index]);
                    max = numClaims[index];
                }
                else if (numClaims[index] == max)
                {
                    claimed.Add(claims[index]);
                }
            }
            return claimed;
        }
    }
}