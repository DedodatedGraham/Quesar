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

        //we will use a quad tree to store the center point of every object to make rendering super quick, rather than checking through everything
        //itll only open up objects which have their own predefined hitboxes& everything else 
        //This should work by having a quad tree with the points of objects and making each object have a quad tree array in it with all nessicary information?:)
        string worldName { get; set; }
        public QuadTree worldObjects { get; set; }
        public List<MapElement> mapElements { get; set;}
        public List<MapElement> rendered { get; set; }

        public Rectangle boundary { get; set; }
        public bool hasSave { get; set; }
        public string saveLocation { get; set; }
        public string saveName { get; set; }

        public Map(GraphicsDevice gd,int xSize, int ySize,string name,ContentManager c)
        {
            rendered = new List<MapElement>();
            mapElements = new List<MapElement>();

            worldName = name;
            boundary = new Rectangle(-xSize / 2, -ySize / 2, xSize, ySize);


            
            worldObjects = new QuadTree(new List<Point>(), boundary);
        }

        public void Draw(SpriteBatch sp)
        {
            for(int i = 0; i < rendered.Count; i++)
            {
                rendered[i].Draw(sp);
            }


        }


        public void update(Vector2 playerPos)
        {  
            //Rendering Logic Goes Here, Updates the Rendering with what is turning active/not and adjusting the rendered list to cointain only the building/obj
           
            







        }

        public void Hits()
        {
            //Hitbox dection

           
        }
    }
}
