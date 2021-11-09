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


    //This clas is to be used for anything that needs a hitbox, easiest to just use rectangles and have them be as flexible as possible and make a way to build them into more complex shapes
    //as well if needed, but hoenstly retangles should be fine and thigns can just hold more than one
    public class Hitbox
    {
        //specific pixel locations to agree with movements,,
        public int tileX { get; set; }
        public int tileY { get; set; }
        public int posTileX { get; set; }
        public int posTileY { get; set; }

        //Size modifications

        public int width { get; set; }
        public int heigth { get; set; }

        //Hitbox Color for drawing if wanted for vizualitation,,
        public Color col = Color.Black;

        public Hitbox(int tx, int ty, int ptx, int pty, int w, int h)
        {
            tileX = tx;
            tileY = ty;
            posTileX = ptx;
            posTileY = pty;
            width = w;
            heigth = h;


        }

        public void UpdateHitbox()
        {

        }
        public Vector2[] GetCorners()
        {
            Vector2[] corn = new Vector2[2];
            //defines top left, & bottom right, gives easy to read boundries
            corn[0] = new Vector2(tileX+posTileX,tileY+posTileY);
            
            corn[3] = new Vector2(tileX + posTileX + width, tileY + posTileY + heigth);

            return corn;
        }







    }
}
