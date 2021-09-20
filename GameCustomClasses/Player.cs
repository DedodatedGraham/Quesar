using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Quesar
{
    public class Player
    {
        //base things, like name,level,exp,health & what not
        public string name{ get; set; }
        public int level { get; set; }
        public double exp { get; set; }
        public int health { get; set; }
        
       
        //things to do with the players ships
        public int maxShip { get; set; }
        public List<Ship> ships { get; set; }



        //Things to do with relative size & location tracking 
        public const int pixelHeight = 64;
        public const int pixelWidth = 32;
        public const int tileHeight = pixelHeight / 32;
        public const int tileWidth = pixelWidth / 32;
        //Things to do with displaying Player
        public Texture2D skin { get; set; }
        public OrthographicCamera cam;
        public GraphicsDevice graphicsDevice { get; set; }


        public Player(GraphicsDevice gd,Texture2D con,string newname)
        {
            graphicsDevice = gd;
            name = newname;
            level = 0;
            exp = 0;
            ships = null;
            maxShip = 1;
            skin = con;
            cam = new OrthographicCamera(graphicsDevice);
        }

        //this adds a ship to the list of character ships, only lets it go if it has space
        public Boolean newShip(Ship newship)
        {
            if (ships.Count < maxShip)
            {
                ships.Add(newship);
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public void Draw()
        {

        }











    }
}
