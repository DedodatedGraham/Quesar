using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    public class Button
    {
        public int xCord { get; set; }
        public int yCord { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        //for determining if the button is on screen
        public bool isActive { get; set; }

        public GraphicsDevice _graphcis;

        public SpriteBatch thisSprite;
        public Texture2D buttonSkin { get; set; }

        public Button(GraphicsDevice gd, int x, int y, int w, int h, Texture2D skin, bool isA)
        {
            //Defines Base properties of the button
            xCord = x;
            yCord = y;
            width = w;
            height = h;

            isActive = isA;

            buttonSkin = skin;
            _graphcis = gd;



            //creates the sprite batch for this Button
            
        }


        public void Draw(SpriteBatch sp)
        {
            

            if (isActive)
            {
                sp.Draw(buttonSkin, new Rectangle(xCord, yCord, width, height), Color.White);
            }



        }






    }
}
