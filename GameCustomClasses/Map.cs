using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    public class Map
    {
        public string mapName;

        protected MapTile mapTile;
        public GraphicsDevice graphicsDevice { get; set; }
        public OrthographicCamera camera { get; }

        public MapElement[] earthBuildings { get; set; }

        public MapTile earthMapTile { get; set; }
        public Map(GraphicsDevice gd,int xSize, int ySize,string name,ContentManager c)
        {
            mapName = name;

            graphicsDevice = gd;
            
            camera = new OrthographicCamera(graphicsDevice);




            //Loading in EarthMap Tiles & buildings
            earthMapTile = new MapTile(graphicsDevice, xSize, ySize);
            earthBuildings = new MapElement[1];
            earthBuildings[0] = new GameCustomClasses.Building(c.Load<Texture2D>("JuliosV1"),1,1);
            //loading whatever is the next map
        }

        public void Draw()
        {





        }

        public void update()
        {

        }


    }
}
