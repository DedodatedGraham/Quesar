using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
     public class Button : UiElement
    {
        public int xCord { get; set; }
        public int yCord { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public string words { get; set; }

        //for determining if the button is on screen
        public override bool isActive { get; set; }

        public GraphicsDevice _graphcis;
        public Texture2D buttonSkin { get; set; }


        public Button(GraphicsDevice gd, int x, int y, int w, int h,string word, Texture2D skin, bool isA)
        {
            //Defines Base properties of the button
            xCord = x;
            yCord = y;
            width = w;
            height = h;

            isActive = isA;

            buttonSkin = skin;
            _graphcis = gd;

            words = word;

            //creates the sprite batch for this Button
            
        }


         public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            Vector2 size = font.MeasureString(words);

            if (isActive)
            {
                sp.Draw(buttonSkin, new Rectangle(xCord, yCord, width, height), Color.White);
                //might need to fiddle with positioning of text later butthis will work for now
                double x = xCord + (width / 2 - size.X / 2);
                double y = yCord + (height / 2 - size.Y / 2);
                x = Math.Floor(x);
                y = Math.Floor(y);
                Point p = new Point((int)x, (int)y);

                sp.DrawString(font,words,p.ToVector2(),Color.Black);
            }



        }

        public override bool isClicked()
        {
            if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (isHovering()))
            {
                return true;
            }


            return false;

        }

        public bool isHovering()
        {
            if(Mouse.GetState().X > xCord && Mouse.GetState().X < (xCord + width) && Mouse.GetState().Y > yCord && Mouse.GetState().Y < (yCord + height))
            {
                return true;
            }



            return false;
        }






    }
}
