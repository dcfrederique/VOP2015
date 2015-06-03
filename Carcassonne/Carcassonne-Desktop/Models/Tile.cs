using System;
using System.Collections.Generic;
using Carcassonne_Desktop.Models.Features;
using GalaSoft.MvvmLight;

namespace Carcassonne_Desktop.Models
{
    public class Tile : ObservableObject
    {
        public List<Tile> Neighbors { get; set; }
        public FeatureType north, south, east, west;
        #region CONSTRUCTORS
        public Tile(Feature nNW, Feature n, Feature nNE,
            Feature eNE, Feature e, Feature eSE,
            Feature sSE, Feature s, Feature sSW,
            Feature wSW, Feature w, Feature wNW,
            Feature center, string texture)
        {
            Features = new List<Feature>(13);
            Features.Add(nNW);
            Features.Add(n);
            north = n.Type;
            Features.Add(nNE);
            Features.Add(eNE);
            Features.Add(e);
            east = e.Type;
            Features.Add(eSE);
            Features.Add(sSE);
            Features.Add(s);
            south = s.Type;
            Features.Add(sSW);
            Features.Add(wSW);
            Features.Add(w);
            west = w.Type;
            Features.Add(wNW);
            Features.Add(center);

            Neighbors = new List<Tile>(8);
            for (var i = 0; i < 8; i++)
            {
                Neighbors.Add(null);
            }

           Texture = texture;
            Location = new Location(0, 0);
            Angle = 0;
            Pawn = new Pawn();
            PawnPosition = FeaturePosition.none;
            Color = "White";
        }

        public Tile(Location l)
        {
            Location = l;
            Color = "White";
            Neighbors = new List<Tile>(8);
            for (var i = 0; i < 8; i++)
            {
                Neighbors.Add(null);
            }
            Visibility = true;
            PawnPosition = FeaturePosition.none;
        }
#endregion CONSTRUCTORS
        #region PROPERTIES
        public Location Location { get; set; }
        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set { Set(() => Enabled, ref enabled, value); }
        }

        private int angle;
        public int Angle
        {
            get { return angle; }
            set { Set(() => Angle, ref angle, value); }
        }

        private String color;

        public String Color
        {
            get { return color; }
            set { Set(() => Color, ref color, value); }
        }

        public int TileID { get; set; }
        public String Texture { get; set; }
        public List<Feature> Features { get; set; }
        public Pawn Pawn { get; set; }
        public FeaturePosition PawnPosition { get; set; }

        private String pawnTexture;
        public String PawnTexture
        {
            get { return pawnTexture; }
            set { Set(() => PawnTexture, ref pawnTexture, value); }
        }

        private int pawnX;
        public int PawnX
        {
            get { return pawnX; }
            set { Set(() => PawnX, ref pawnX, value); }
        }

        private int pawnY;
        public int PawnY
        {
            get { return pawnY; }
            set { Set(() => PawnY, ref pawnY, value); }
        }

        private bool visibility;
        public bool Visibility
        {
            get { return visibility; }
            set { Set(() => Visibility, ref visibility, value); }
        }
        #endregion PROPERTIES
        #region METHODS
        public Feature GetFeature(FeaturePosition position)
        {
            return Features[(int) position];
        }

        public void SetFeature(FeaturePosition pos, Feature f)
        {
            Features.RemoveAt((int) pos);
            Features.Insert((int) pos, f);
        }

        public Tile GetNeighbor(FeaturePosition direction)
        {
            return Neighbors[(int) direction];
        }

        public void SetNeighbor(Tile newTile, TileDirection direction)
        {
            Neighbors[(int) direction] = newTile;
            OnNewNeighbor(newTile, direction);
        }

        public bool IsPossibleNeighbor(Tile newTile, TileDirection direction)
        {
            if (newTile == null)
                return true;
            return Neighbors[(int) direction] == null && CanAttach(newTile, direction);
        }

        public virtual bool CanAttach(Tile newTile, TileDirection side)
        {
            switch (side)
            {
                case TileDirection.east:
                    if (
                        !Feature.IsSameType(Features[(int) FeaturePosition.eastNorthEast],
                            newTile.Features[(int) FeaturePosition.westNorthWest]))
                        return false;
                    if (
                        !Feature.IsSameType(Features[(int) FeaturePosition.east],
                            newTile.Features[(int) FeaturePosition.west]))
                        return false;
                    return Feature.IsSameType(Features[(int) FeaturePosition.eastSouthEast],
                        newTile.Features[(int) FeaturePosition.westSouthWest]);
                case TileDirection.west:
                    if (
                        !Feature.IsSameType(Features[(int) FeaturePosition.westNorthWest],
                            newTile.Features[(int) FeaturePosition.eastNorthEast]))
                        return false;
                    if (
                        !Feature.IsSameType(Features[(int) FeaturePosition.west],
                            newTile.Features[(int) FeaturePosition.east]))
                        return false;
                    return Feature.IsSameType(Features[(int) FeaturePosition.westSouthWest],
                        newTile.Features[(int) FeaturePosition.eastSouthEast]);
                case TileDirection.north:
                    if (
                        !Feature.IsSameType(Features[(int) FeaturePosition.northNorthEast],
                            newTile.Features[(int) FeaturePosition.southSouthEast]))
                        return false;
                    if (
                        !Feature.IsSameType(Features[(int) FeaturePosition.north],
                            newTile.Features[(int) FeaturePosition.south]))
                        return false;
                    return Feature.IsSameType(Features[(int) FeaturePosition.northNorthWest],
                        newTile.Features[(int) FeaturePosition.southSouthWest]);
                case TileDirection.south:
                    if (
                        !Feature.IsSameType(Features[(int) FeaturePosition.southSouthEast],
                            newTile.Features[(int) FeaturePosition.northNorthEast]))
                        return false;
                    if (
                        !Feature.IsSameType(Features[(int) FeaturePosition.south],
                            newTile.Features[(int) FeaturePosition.north]))
                        return false;
                    return Feature.IsSameType(Features[(int) FeaturePosition.southSouthWest],
                        newTile.Features[(int) FeaturePosition.northNorthWest]);
                default:
                    return false;
            }
        }

        public bool CombineFeatures(TileDirection direction, Tile newTile)
        {
            switch (direction)
            {
                case TileDirection.east:
                    if (
                        !Features[(int) FeaturePosition.eastNorthEast].Combine(
                            newTile.Features[(int) FeaturePosition.westNorthWest]))
                        throw new Exception("Fail");
                    if (!Features[(int) FeaturePosition.east].Combine(newTile.Features[(int) FeaturePosition.west]))
                        throw new Exception("Fail");
                    if (
                        !Features[(int) FeaturePosition.eastSouthEast].Combine(
                            newTile.Features[(int) FeaturePosition.westSouthWest]))
                        throw new Exception("Fail");
                    return true;
                case TileDirection.west:
                    if (
                        !Features[(int) FeaturePosition.westNorthWest].Combine(
                            newTile.Features[(int) FeaturePosition.eastNorthEast]))
                        throw new Exception("Fail");
                    if (!Features[(int) FeaturePosition.west].Combine(newTile.Features[(int) FeaturePosition.east]))
                        throw new Exception("Fail");
                    if (
                        !Features[(int) FeaturePosition.westSouthWest].Combine(
                            newTile.Features[(int) FeaturePosition.eastSouthEast]))
                        throw new Exception("Fail");
                    return true;
                case TileDirection.north:
                    if (
                        !Features[(int) FeaturePosition.northNorthEast].Combine(
                            newTile.Features[(int) FeaturePosition.southSouthEast]))
                        throw new Exception("Fail");
                    if (!Features[(int) FeaturePosition.north].Combine(newTile.Features[(int) FeaturePosition.south]))
                        throw new Exception("Fail");
                    if (
                        !Features[(int) FeaturePosition.northNorthWest].Combine(
                            newTile.Features[(int) FeaturePosition.southSouthWest]))
                        throw new Exception("Fail");
                    return true;
                case TileDirection.south:
                    if (
                        !Features[(int) FeaturePosition.southSouthEast].Combine(
                            newTile.Features[(int) FeaturePosition.northNorthEast]))
                        throw new Exception("Fail");
                    if (!Features[(int) FeaturePosition.south].Combine(newTile.Features[(int) FeaturePosition.north]))
                        throw new Exception("Fail");
                    if (
                        !Features[(int) FeaturePosition.southSouthWest].Combine(
                            newTile.Features[(int) FeaturePosition.northNorthWest]))
                        throw new Exception("Fail");
                    return true;
                default:
                    throw new Exception("Error");
            }
        }

        public void OnCombinedFeature(Feature oldFeature, Feature newFeature)
        {
            int index;
            while ((index = Features.IndexOf(oldFeature)) >= 0)
            {
                Features[index] = newFeature;
            }
        }

        public void MarkSelected()
        {
            Color = "Yellow";
        }

        public void OnNewNeighbor(Tile newNeighbor, TileDirection direction)
        {
            var uniqueFeatures = new List<Feature>();
            foreach (var feature in Features)
            {
                if (!uniqueFeatures.Contains(feature))
                {
                    uniqueFeatures.Add(feature);
                }
            }
            // Tell each feature it has a new neighbor
            foreach (var feature in uniqueFeatures)
                feature.OnNewNeighbor(newNeighbor, direction);

            uniqueFeatures.Clear();
            Neighbors.Add(newNeighbor);
        }

        public void Rotate_right()
        {
            var temp1 = new List<Feature>(Features);
            var temp2 = new List<Feature>(Features);

            //Remove the center feature.
            temp1.RemoveAt(temp1.Count - 1);
            temp2.RemoveAt(temp2.Count - 1);

            //Rotate the Features with 3 places
            for (var i = 0; i < temp1.Count; i++)
            {
                temp1[(i + 3)%temp1.Count] = temp2[i];
            }
            //Add the center feature again
            temp1.Add(Features[Features.Count - 1]);
            Features = temp1;

            //Assign new Features
            var temp_type = north;
            north = west;
            west = south;
            south = east;
            east = temp_type;
        }

        public void Rotate_left()
        {
            var temp1 = new List<Feature>(Features);
            var temp2 = new List<Feature>(Features);

            //Remove the center feature.
            temp1.RemoveAt(temp1.Count - 1);
            temp2.RemoveAt(temp2.Count - 1);

            //Rotate the Features with 3 places
            for (var i = 0; i < temp1.Count; i++)
            {
                temp1[i] = temp2[(i + 3)%temp2.Count];
            }

            //Add the center feature again
            temp1.Add(Features[Features.Count - 1]);
            Features = temp1;

            //Assign new Features
            var temp_type = north;
            north = east;
            east = south;
            south = west;
            west = temp_type;
        }

        public void Place_pawn(FeaturePosition pos, Pawn p)
        {
            Pawn = p;
            pawnTexture = Pawn.Player.Avatar;
            PawnPosition = pos;
            switch (PawnPosition)
            {
                case FeaturePosition.north:
                    pawnX = 1;
                    pawnY = 0;
                    break;
                case FeaturePosition.northNorthEast:
                case FeaturePosition.eastNorthEast:
                    pawnX = 2;
                    pawnY = 0;
                    break;
                case FeaturePosition.east:
                    pawnX = 2;
                    pawnY = 1;
                    break;
                case FeaturePosition.eastSouthEast:
                case FeaturePosition.southSouthEast:
                    pawnX = 2;
                    pawnY = 2;
                    break;
                case FeaturePosition.south:
                    pawnX = 1;
                    pawnY = 2;
                    break;
                case FeaturePosition.southSouthWest:
                case FeaturePosition.westSouthWest:
                    pawnX = 0;
                    pawnY = 2;
                    break;
                case FeaturePosition.west:
                    pawnX = 0;
                    pawnY = 1;
                    break;
                case FeaturePosition.northNorthWest:
                case FeaturePosition.westNorthWest:
                    pawnX = 0;
                    pawnY = 0;
                    break;
                case FeaturePosition.center:
                    pawnX = 1;
                    pawnY = 1;
                    break;
                case FeaturePosition.none:
                    pawnX = -1;
                    pawnY = -1;
                    break;
                default:
                    break;
            }
        }
        #endregion METHODS

    }
}