using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar.GameCustomClasses
{
    public class Building : MapElement
    {

        public Texture2D skin { get; set; }

        public string name { get; set; }

        public override int tileX { get; set; }
        public override int tileY { get; set; }

        public override bool isActive { get; set; }
        public Rectangle rectangle { get; set; }
        public Building(Texture2D sk,int x,int y){

            skin = sk;


            tileX = x;
            tileY = y;


            rectangle = new Rectangle(tileX*32,tileY*32, skin.Width*2, skin.Height*2);


        }



        public override void Draw(SpriteBatch sp)
        {
            if (isActive)
            {
                sp.Draw(skin, rectangle, Color.White);
            }

        }
    }
}