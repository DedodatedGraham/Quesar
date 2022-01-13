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
        public int limit { get; set; }

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


            int limit = 0;
            int max = 10000;
            if (boundary.Width > max|| boundary.Height > max)
            {
                limit = max / 100;
            }
            else
            {
                if(boundary.Width >= boundary.Height)
                {
                    limit = boundary.Width / 100;
                }
                else
                {
                    limit = boundary.Height / 100;
                }
            }

            //side note for later make sure the empty list works, could potentially be a problem but could just work fine
            worldObjects = new QuadTree(new List<MyPoint>(), boundary, "objects");

        }

        public void Draw(SpriteBatch sp)
        {


            for(int i = 0; i < rendered.Count; i++)
            {
                rendered[i].Draw(sp);
            }


        }


        public void update(Vector2 playerPos,int renderDistance)
        {
            //Rendering Logic Goes Here, Updates the Rendering with what is turning active/not and adjusting the rendered list to cointain only the building/obj

            Render(playerPos,renderDistance);






            worldObjects.update();
        }

        public void Render(Vector2 main,int rd)
        {
            //properly updates render size to be correct and with the character
            int Size = rd * limit;
            Rectangle curbounds = new Rectangle((int)main.X - Size / 2, (int)main.Y - Size / 2, Size, Size);
            MyPoint NorthWest = new MyPoint(curbounds.X, curbounds.Y, "render");
            MyPoint SouthWest = new MyPoint(curbounds.X, curbounds.Y + curbounds.Height, "render");
            MyPoint NorthEast = new MyPoint(curbounds.X + curbounds.Width, curbounds.Y, "render");
            MyPoint SouthEast = new MyPoint(curbounds.X + curbounds.Width, curbounds.Y + curbounds.Height, "render");

            //now with corners set we need to group every object with eachother thats in those bounds

            loadRendered(NorthWest,SouthWest,NorthEast,SouthEast);

        }
        public void Hits()
        {
            //Hitbox dection

           
        }

        private void loadRendered(MyPoint nw, MyPoint sw, MyPoint ne, MyPoint se)
        {
            List<MyPoint> loaded = worldObjects.gatherNear(nw,sw,ne,se);

        }
    }
}
