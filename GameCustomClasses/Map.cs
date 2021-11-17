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

        protected MapTile mapTile;
        public GraphicsDevice graphicsDevice { get; set; }
        public OrthographicCamera camera { get; }

        public MapElement[][] universe { get; set; }
        public MapElement[] earthBuildings { get; set; }

        //keeps track of elements rendered in
        public List<int> rendered { get; set; }

        public MapTile earthMapTile { get; set; }
        public Map(GraphicsDevice gd,int xSize, int ySize,string name,ContentManager c)
        {
            mapName = name;

            graphicsDevice = gd;
            
            camera = new OrthographicCamera(graphicsDevice);

            mapStage = 0;

            mapTile = new MapTile(gd, 300, 300);

            //Loading in EarthMap Tiles & buildings
            earthMapTile = new MapTile(graphicsDevice, xSize, ySize);
            earthBuildings = new MapElement[1];
            earthBuildings[0] = new GameCustomClasses.Building(c.Load<Texture2D>("JuliosV1"),1,1,20,20);
            //loading whatever is the next map


            universe = new MapElement[1][];
            universe[0] = earthBuildings;

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
                    universe[mapStage-1][rendered[i]].Draw(sp);
                    i++;
                }
                mapTile.Draw(sp);
            }


        }

        public void update(Vector2 playerTile)
        {

            int rL = 40;
            //Rendering Logic Goes Here, Updates the Rendering with what is turning active/not and adjusting the rendered list to cointain only the building/obj
            int i = 0;
            if(mapStage != 0)
            {
                while (i < universe[mapStage - 1].Length)
                {
                    bool shouldActive = (playerTile.X - universe[mapStage - 1][i].tileX <= rL && playerTile.X - universe[mapStage - 1][i].tileX >= -rL) && (playerTile.Y - universe[mapStage - 1][i].tileY <= rL && playerTile.Y - universe[mapStage - 1][i].tileY >= -rL);
                    //Checks if active & shouldnt be
                    if (universe[mapStage - 1][i].isActive && !shouldActive)
                    {
                        universe[mapStage - 1][i].isActive = false;
                        rendered.Remove(i);
                    }
                    //checks if not active & should be
                    if (!universe[mapStage - 1][i].isActive && shouldActive)
                    {
                        universe[mapStage - 1][i].isActive = true;
                        rendered.Add(i);
                    }
                    i++;
                }
            }
            







        }

        public GameCustomClasses.Hitbox[] activeHits()
        {
            
            int i = 0;

            List<GameCustomClasses.Hitbox> send = new List<GameCustomClasses.Hitbox>();

            while (i < rendered.Count)
            {
                    
                
                
                send.Add(universe[mapStage-1][rendered[i]].hitbox);
                    
                

                i++;
            }




            return send.ToArray();
        }
    }
}
