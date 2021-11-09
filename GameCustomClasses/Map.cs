﻿using System;
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

        public MapTile earthMapTile { get; set; }
        public Map(GraphicsDevice gd,int xSize, int ySize,string name,ContentManager c)
        {
            mapName = name;

            graphicsDevice = gd;
            
            camera = new OrthographicCamera(graphicsDevice);

            mapStage = 0;


            //Loading in EarthMap Tiles & buildings
            earthMapTile = new MapTile(graphicsDevice, xSize, ySize);
            earthBuildings = new MapElement[1];
            earthBuildings[0] = new GameCustomClasses.Building(c.Load<Texture2D>("JuliosV1"),1,1,20,20);
            //loading whatever is the next map


            universe = new MapElement[1][];
            universe[0] = earthBuildings;
        }

        public void Draw(SpriteBatch sp)
        {

            if(mapStage == 1)
            {
                int i = 0;
                while (i < earthBuildings.Length)
                {
                    //replace the 10 with some math to calulate how close the x & y need to be to render(this is the rendeing logic, should be fairly simple bc just checking if
                    //the x & y are accurate, and if the isActive is false, then it wont draw the picture. bc of self check :p
                    //mighjt move rendering into its own thing though so it can be used by other things easier
                    if (earthBuildings[i].tileX < render && earthBuildings[i].tileY < render)
                    {

                        earthBuildings[i].isActive = true;
                    }
                    else
                    {
                        earthBuildings[i].isActive = false;
                    }
                    earthBuildings[i].Draw(sp);
                    i++;
                }
            }


        }

        public void update()
        {

        }

        public GameCustomClasses.Hitbox[] activeHits()
        {
            
            int i = 0;

            List<GameCustomClasses.Hitbox> send = new List<GameCustomClasses.Hitbox>();

            while (i < universe[mapStage -1].Length)
            {
                if(universe[mapStage -1][i].isActive == true)
                {
                    send.Add(universe[mapStage-1][i].hitbox);
                    
                }

                i++;
            }




            return send.ToArray();
        }
    }
}
