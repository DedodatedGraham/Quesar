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

        public GameCustomClasses.Building julios { get; set; }

        public Map(GraphicsDevice gd,int xSize, int ySize,string name,ContentManager c)
        {
            mapName = name;

            graphicsDevice = gd;
            mapTile = new MapTile(graphicsDevice, xSize, ySize);
            camera = new OrthographicCamera(graphicsDevice);

            julios = new GameCustomClasses.Building(c.Load<Texture2D>("JuliosV1"));
        }

        public void Draw()
        {





        }


    }
}
