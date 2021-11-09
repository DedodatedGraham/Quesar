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
        //things with player location
        public bool isActive { get; set; }
        public int world { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Rectangle rectangle { get; set; }
       
        //things to do with the players ships
        public int maxShip { get; set; }
        public List<Ship> ships { get; set; }

        //HitBox
        public GameCustomClasses.Hitbox hitbox { get; set; }



        
        //Things to do with displaying Player
        public Texture2D skin { get; set; }
        public OrthographicCamera cam;
        public GraphicsDevice graphicsDevice { get; set; }



        public Player(GraphicsDeviceManager gdm,GraphicsDevice gd,Texture2D con,string newname)
        {
            graphicsDevice = gd;
            name = newname;
            level = 0;
            exp = 0;
            ships = null;
            maxShip = 1;
            isActive = false;
            world = 1;
            skin = con;
            x = gdm.PreferredBackBufferWidth/2-skin.Width/2;
            y = gdm.PreferredBackBufferHeight / 2 - skin.Height / 2;
            rectangle = new Rectangle(x,y,skin.Width,skin.Height);

            hitbox = new GameCustomClasses.Hitbox(0,0,0,0,skin.Width,skin.Height);


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

        

        public void Draw(SpriteBatch sp)
        {


            sp.Begin();


            sp.Draw(skin, new Size2(x,y), Color.White);

            sp.End();

        }


        public void updatePlayer()
        {   


        }








    }
}
