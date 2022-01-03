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

        public int mapStage { get; set; }

        public const int render = 10;

        public GraphicsDevice graphicsDevice { get; set; }
        public OrthographicCamera camera { get; }

        
        //keeps track of elements rendered in
        public List<int> rendered { get; set; }

        public Map(GraphicsDevice gd,int xSize, int ySize,string name,ContentManager c)
        {
            mapName = name;

            graphicsDevice = gd;
            
            camera = new OrthographicCamera(graphicsDevice);

            mapStage = 0;


            //Loading in EarthMap Tiles & buildings
            //loading whatever is the next map


            

            rendered = new List<int>();
        }

        public void Draw(SpriteBatch sp)
        {
            //Draws anything that is rendered into the game on the given map that is supposed to be rendered
            if(mapStage != 0)
            {
                int i = 0;
                while (i < rendered.Count)
                {
                    
                }
            }


        }

        public void update()
        {

            int rL = 40;
            //Rendering Logic Goes Here, Updates the Rendering with what is turning active/not and adjusting the rendered list to cointain only the building/obj
            int i = 0;
            if(mapStage != 0)
            {
            }
            







        }

        public void Hits()
        {
            //Hitbox dection
           
        }
    }
}
