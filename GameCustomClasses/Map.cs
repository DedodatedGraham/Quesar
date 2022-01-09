using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
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
        //universe is a matrix, each of a world with its own containing of all the different quadtrees 
        //4 types per map with a
        //Player point(moving with hitbox)
        //Mapelement point(static with hitbox)
        //effect point(moving no hitbox)
        //background point(static no hitbox)
        public QuadTree[][] universe { get; set; }
        public int mapStage { get; set; }
        public bool hasSave { get; set; }
        public string saveLocation { get; set; }

        public List<MapElement> things { get; set; }

        public Map(GraphicsDevice gd,int xSize, int ySize,string name,ContentManager c)
        {
            


            

        }

        public void Draw(SpriteBatch sp)
        {
            //Draws anything that is rendered into the game on the given map that is supposed to be rendered
            


        }

        public void update()
        {

           
            //Rendering Logic Goes Here, Updates the Rendering with what is turning active/not and adjusting the rendered list to cointain only the building/obj
           
            







        }

        public void Hits()
        {
            //Hitbox dection
           
        }
    }
}
