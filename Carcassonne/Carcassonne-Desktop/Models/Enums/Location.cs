namespace Carcassonne_Desktop.Models
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
       
        public int ViewX
        {
            // 80 is grootte van de tile, 500 is de x positie van de starttile in pixelcoordinaten
            get { return X * 80 + (int)System.Windows.SystemParameters.PrimaryScreenWidth/3; }
        }
        public int ViewY
        {
            get { return Y * 80 + (int)System.Windows.SystemParameters.PrimaryScreenHeight/2 -85; }
        }

    }
}