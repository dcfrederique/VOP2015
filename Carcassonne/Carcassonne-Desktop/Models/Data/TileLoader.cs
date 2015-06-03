using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Carcassonne_Desktop.Models;
using System.IO;
using Carcassonne_Desktop.Models.Features;

namespace Carcassonne_Desktop.Models.Data
{
    public class TileLoader
    {
        private static TileLoader _instance = new TileLoader();

        public static TileLoader getInstance
        {
            get
            {
                return _instance;
            }
        }

        public List<Tile> LoadTiles(StringReader file)
        {
            List<Tile> returnList = new List<Tile>();
            List<Feature[]> featureList = new List<Feature[]>();
            List<int> directionList = new List<int>();
            string pos, texture = "";
            int id, shield,openings = 0;
            int nr = 0;
            Tile temp_tile=null;
            Feature[] temp_feat = null;
            int tileID = 0;


            XmlTextReader reader = new XmlTextReader(file);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            
            while(reader.Read())
            {
                if(reader.NodeType == XmlNodeType.Element)
                {
                    if(reader.LocalName.Equals("tile"))
                    {
                        nr = Int32.Parse(reader.GetAttribute("number"));
                    }
                    if(reader.LocalName.Equals("cloister"))
                    {
                        id = Int32.Parse(reader.GetAttribute("id"));
                        temp_feat = new Feature[nr];
                        for(int i=0;i<nr;i++)
                        {
                            temp_feat[i] = new Monastery(id);
                        }
                        featureList.Add(temp_feat);
                    }
                    if(reader.LocalName.Equals("farm"))
                    {
                        id = Int32.Parse(reader.GetAttribute("id"));
                        temp_feat = new Feature[nr];
                        for(int i=0;i<nr;i++)
                        {
                            temp_feat[i] = new Farm(id);
                        }
                        featureList.Add(temp_feat);
                    }
                    if(reader.LocalName.Equals("neighbor_city"))
                    {
                        int cityID = Int32.Parse(reader.ReadString());
                        for(int i=0;i<nr;i++)
                        {
                            Farm t = (Farm)featureList[featureList.Count - 1][i];
                            City city = null;
                            foreach (Feature[] f in featureList)
                                if (f[i].ID == cityID)
                                    city = (City)f[i];
                            if (city == null) //error
                                 t.Cities.Add(city);
                        }
                    }
                    if(reader.LocalName.Equals("road"))
                    {
                        id = Int32.Parse(reader.GetAttribute("id"));
                        reader.Read();
                        if(reader.LocalName.Equals("openings"))
                        {
                            openings = Int32.Parse(reader.ReadString());
                        }
                        temp_feat = new Feature[nr];
                        for(int i=0;i<nr;i++)
                        {
                            temp_feat[i] = new Road(id, openings);
                        }
                        featureList.Add(temp_feat);
                    }

                    if(reader.LocalName.Equals("city"))
                    {
                        id = Int32.Parse(reader.GetAttribute("id"));
                        reader.Read();
                        openings = Int32.Parse(reader.ReadString());
                        reader.Read();
                        shield = Int32.Parse(reader.ReadString());
                        temp_feat = new Feature[nr];
                        for(int i=0;i<nr;i++)
                        {
                            temp_feat[i] = new City(id, openings, shield);
                        }
                        featureList.Add(temp_feat);

                    }
                    if(reader.LocalName.Equals("feature"))
                    {
                        pos = reader.GetAttribute("direction");
                        directionList.Add(Int32.Parse(reader.ReadString()));
                    }
                    if(reader.LocalName.Equals("texture"))
                    {
                        texture = reader.ReadString();
                    }
                }
                if(reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.LocalName.Equals("tile"))
                    {
                        for (int i = 0; i < nr; i++)
                        {
                            temp_tile = new Tile(featureList[directionList[0]][i], featureList[directionList[1]][i], featureList[directionList[2]][i],
                                    featureList[directionList[3]][i], featureList[directionList[4]][i], featureList[directionList[5]][i], featureList[directionList[6]][i],
                                    featureList[directionList[7]][i], featureList[directionList[8]][i], featureList[directionList[9]][i], featureList[directionList[10]][i], featureList[directionList[11]][i],
                                    (directionList[12] != -1) ? featureList[directionList[12]][i] : null, texture); 
                            returnList.Add(temp_tile);

                            temp_tile.TileID = tileID++;

                            Feature[] test = featureList[directionList[0]];
                            Feature[] test2 = featureList[directionList[1]];

                            foreach (Feature[] feature in featureList)
                                feature[i].AddTile(temp_tile);
                        }

                        featureList.Clear();
                        directionList.Clear();
                    }
                 }
            }
            return returnList;
        }

    }
}
