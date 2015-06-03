using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Carcassonne_Desktop.Models
{
    public class Board
    {
        public Board()
        {
            Tiles = new ObservableCollection<Tile>();
        }

        public ObservableCollection<Tile> Tiles { get; set; }

        public bool SearchTile(int x, int y)
        {
            return Tiles.Any(t => t.Location.X == x && t.Location.Y == y);
        }

        public bool IsTileSet(int x, int y)
        {
            return Tiles.Any(t => t.Location.X == x && t.Location.Y == y && t.Texture != null);
        }

        public void MakePossibleLeftTile(Tile tile)
        {
            if (!SearchTile(tile.Location.X - 1, tile.Location.Y))
            {
                Tiles.Add(new Tile(new Location(tile.Location.X - 1, tile.Location.Y)));
            }
        }

        public void MakePossibleRightTile(Tile tile)
        {
            if (!SearchTile(tile.Location.X + 1, tile.Location.Y))
            {
                Tiles.Add(new Tile(new Location(tile.Location.X + 1, tile.Location.Y)));
            }
        }

        public void MakePossibleUpTile(Tile tile)
        {
            if (!SearchTile(tile.Location.X, tile.Location.Y + 1))
            {
                Tiles.Add(new Tile(new Location(tile.Location.X, tile.Location.Y + 1)));
            }
        }

        public void MakePossibleBottomTile(Tile tile)
        {
            if (!SearchTile(tile.Location.X, tile.Location.Y - 1))
            {
                Tiles.Add(new Tile(new Location(tile.Location.X, tile.Location.Y - 1)));
            }
        }

        public void ClearEmptyTiles()
        {
            for (var i = Tiles.Count - 1; i >= 0; i--)
            {
                if (Tiles[i].Texture == null)
                {
                    Tiles.RemoveAt(i);
                }
            }
        }

        public void AddTile(Tile t)
        {
            Tile old = Tiles.FirstOrDefault(q => q.Location.X == t.Location.X && q.Location.Y == t.Location.Y);
            if(old!=null)
            {
                Tiles.RemoveAt(Tiles.IndexOf(old));
            }
            Tiles.Add(t);
        }

        public bool IsPlaceValid(Tile emptyTile, Tile nextTile)
        {
            var visible = true;
            var left = GetTile(emptyTile.Location.X - 1, emptyTile.Location.Y);
            if (left != null)
            {
                //check
                visible = left.IsPossibleNeighbor(nextTile, TileDirection.east);
            }
            var right = GetTile(emptyTile.Location.X + 1, emptyTile.Location.Y);
            if (right != null && visible)
            {
                //check
                visible = right.IsPossibleNeighbor(nextTile, TileDirection.west);
            }
            var up = GetTile(emptyTile.Location.X, emptyTile.Location.Y + 1);
            if (up != null && visible)
            {
                visible = up.IsPossibleNeighbor(nextTile, TileDirection.north);
            }
            var down = GetTile(emptyTile.Location.X, emptyTile.Location.Y - 1);
            if (down != null && visible)
            {
                visible = down.IsPossibleNeighbor(nextTile, TileDirection.south);
            }
            if (emptyTile.Visibility)
            {
                emptyTile.Visibility = visible;
            }
            return emptyTile.Visibility;
        }

        public Tile GetTile(int x, int y)
        {
            return Tiles.FirstOrDefault(t => t.Location.X == x && t.Location.Y == y && t.Texture != null);
        }
    }
}