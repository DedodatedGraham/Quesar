using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quesar
{
    public class Map
    {
        public string mapName;

        protected MapTile mapTile;
        public GraphicsDevice graphicsDevice { get; set; }
        public OrthographicCamera camera { get; }

        public Map(GraphicsDevice gd,int xSize, int ySize,string name)
        {
            mapName = name;

            graphicsDevice = gd;
            mapTile = new MapTile(graphicsDevice, xSize, ySize);
            camera = new OrthographicCamera(graphicsDevice);
        }

        public void Draw()
        {
            mapTile.Draw(camera);
        }


    }
}
