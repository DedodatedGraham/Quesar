using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quesar
{
    public class Ship
    {
        public int speedMax { get; set; }// m/s
        public int speed { get; set; }// m/s
        public int acceleration { get; set; }// m/s^2
        public int armor { get; set; }
        public int shield { get; set; }
        public int strucInteg { get; set; }
        //public engine { get; set; }
        //public parts { get; set; }
        //public turrets { get; set; }
        public GraphicsDevice graphicsDevice { get; set; }
        public OrthographicCamera camera { get; }
        public Texture2D shiptexture { get; set; }
        public SamplerState samp { get; set; }
        public SpriteBatch spriteBatch { get; set; }

        //ill add in these later but they need to have their own classes with their own shit 
        public Ship(GraphicsDevice gd,int spm,int sp,int ac,int ar,int sh,int si,int xTile,int yTile){//,en,pa,tu){
            
            speedMax = spm;
            speed = sp;
            acceleration = ac;
            armor = ar;
            shield = sh;
            strucInteg = si;
            graphicsDevice = gd;
            camera = new OrthographicCamera(graphicsDevice);
            spriteBatch = new SpriteBatch(gd);
            //this.engine = en;
            //this.parts = pa;
            //this.turrets = tu;
        
        }
        public void Draw()
        {
            
            
            //this is the draw method for everything to do with the ship
            


            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, transformMatrix: camera.GetViewMatrix());

            spriteBatch.Draw(shiptexture,new Size2(0, 0), Color.White);

            spriteBatch.End();

            //shipTile.Draw(camera);

        }

    }
}
