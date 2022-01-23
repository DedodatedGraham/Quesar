using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Xml.Serialization;

namespace Quesar
{
    [Serializable()]
    public class Map
    {

        //we will use a quad tree to store the center point of every object to make rendering super quick, rather than checking through everything
        //itll only open up objects which have their own predefined hitboxes& everything else 
        //This should work by having a quad tree with the points of objects and making each object have a quad tree array in it with all nessicary information?:)
        public string worldName { get; set; }
        public QuadTree worldObjects { get; set; }
        public List<MapElement> mapElements { get; set;}
        [XmlIgnore]
        private List<MapElement> rendered { get; set; }
        public int limit { get; set; }

        public Rectangle boundary { get; set; }
        public bool hasSave { get; set; }
        public string saveLocation { get; set; }
        public string saveName { get; set; }
        [XmlIgnore]
        private Texture2D julios;
        [XmlIgnore]
        private ContentManager content { get; set; }

        [XmlIgnore]
        private Texture2D enviormentSheet { get; set; }
        public string sheetName { get; set; }

        public Map(GraphicsDevice gd,int xSize, int ySize,string name,ContentManager c)
        {
            content = c;
            rendered = new List<MapElement>();
            mapElements = new List<MapElement>();


            julios = c.Load<Texture2D>("JuliosV1");

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
            worldObjects = new QuadTree(new List<MyPoint>(), boundary, "location");

        }

        public Map(ContentManager c)
        {
            julios = c.Load<Texture2D>("JuliosV1");
            rendered = new List<MapElement>();
            mapElements = new List<MapElement>();
            
            boundary = new Rectangle(0,0,1000,1000);
            worldObjects = new QuadTree();
            saveName = "";
        }
        public Map()
        {

        }


        public void Draw(SpriteBatch sp)
        {

            //temp for displaying the map quad tree and traces if an object has them,
            //trace draw will show lines and polygons know intersections aswell
            if(worldObjects.getCount() != 0)
            {

                worldObjects.Draw(sp);
                if(!(worldObjects.traces is null) && worldObjects.traces.Count != 0)
                {
                    worldObjects.DrawTrace(sp);
                }
            }
            //

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
            //this will update the mapelements "rendered" to be correct so now the rendered just needs to be drawn and it will auto maintain
            loadRendered(NorthWest,SouthWest,NorthEast,SouthEast);

        }
        public void Hits()
        {
            //Hitbox dection

           
        }

        private void loadRendered(MyPoint nw, MyPoint sw, MyPoint ne, MyPoint se)
        {
            //gathers location of all needed objects
            List<MyPoint> loaded = new List<MyPoint>();
            loaded = worldObjects.gatherNear(nw,sw,ne,se);
            List<MapElement> temp = new List<MapElement>();

            int orgIndex = 0;
            //will skip if there is nothing to be loaded
            if (!(loaded is null) && loaded.Count != 0)
            {
                //goes through and finds all the sam elements at the front end to keep
                for (int i = 0; i < rendered.Count; i++)
                {
                    if (i < loaded.Count)
                    {
                        if (loaded[i].X == rendered[i].location.X && loaded[i].Y == rendered[i].location.Y && loaded[i].id == rendered[i].location.id)
                        {
                            orgIndex++;
                        }
                        else
                        {
                            break;
                        }
                    }

                }


                //now needs to scan for rendered objects that shouldnt be, and unrendered objects that should be

                if (orgIndex < loaded.Count)
                {
                    //this will now run through each element which isnt the same from the beggining 
                    //org index will keep it from needing to look at the first same elements
                    for (int i = orgIndex; i < loaded.Count; i++)
                    {
                        for (int j = 0; j < rendered.Count; j++)
                        {
                            //this will keep elements that exist in rendered already to just be added back into the rendered
                            if (loaded[i].id == rendered[j].location.id)
                            {
                                temp.Add(rendered[j]);
                                break;
                            }
                            if (j == rendered.Count - 1)
                            {
                                temp.Add(getElement(loaded[i]));
                            }
                        }
                    }
                    if (orgIndex != 0)
                    {
                        rendered = (List<MapElement>)rendered.GetRange(0, orgIndex - 1);
                        for(int i = 0;i< temp.Count; i++)
                        {
                            rendered.Add(temp[i]);
                        }
                    }
                    else
                    {
                        rendered = temp;
                    }
                }
            }
            
            //if its bigger it wont change becaause that will mean its the same rendered so it stays
            //and if its both 0 itll catch it below
            
            

            

            //if theres nothing rendered it will set its self to loaded
            if (rendered.Count == 0)
            {
                for (int i = 0; i < loaded.Count; i++)
                {
                    rendered.Add(getElement(loaded[i]));
                }
            }

        }

        private void initializeElements()
        {
            

        }

        private MapElement getElement(MyPoint point)
        {
            //this will search through the point data base and send out a properly formatted MapElement
            Building juli = new Building(julios, new Rectangle(point.X - julios.Width/2,point.Y - julios.Height/2, julios.Height, julios.Width),true,new List<QuadTree>());
            juli.location = point;
            
            return juli;
        }

        public void setSprite(Texture2D sheet)
        {
            enviormentSheet = sheet;
        }
    }
}
