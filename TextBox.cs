using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    public class TextBox : UiElement
    {
        public int xCord { get; set; }
        public int yCord { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int maxLength { get; set; }

        public string words { get; set; }
        public string typed { get; set; }

        //for determining if the button is on screen
        public override bool isActive { get; set; }
        public bool isTyping { get; set; }

        public GraphicsDevice _graphcis;
        public Texture2D buttonSkin { get; set; }

        public TextBox(GraphicsDevice gd, int x, int y, int w, int h,int ml,string word,Texture2D skin, bool isA)
        {
            xCord = x;
            yCord = y;
            width = w;
            height = h;

            maxLength = ml;

            isActive = isA;

            buttonSkin = skin;
            _graphcis = gd;

            words = word;
            typed = "test";
            isTyping = false;



        }


        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            if (isActive)
            {
                Rectangle r = new Rectangle();
                r.X = xCord + width / 10;
                r.Y = yCord + (55/100)*height;
                r.Width = (8 / 10) * width;
                r.Height = (35 / 100) * height;


                

                Vector2 size = font.MeasureString(words);
                Vector2 size2 = font.MeasureString(typed);

                sp.Draw(buttonSkin, new Rectangle(xCord, yCord, width, height), Color.White);
                sp.DrawRectangle(r, Color.White);
                sp.DrawString(font, words, new Vector2(xCord + (width / 2 - size.X / 2), yCord + (height / 10)), Color.Black);
             
                sp.DrawString(font, typed, new Vector2(xCord + (width / 2 - size2.X / 2), yCord + (height * (55 / 100) + size2.Y)), Color.Black);
                



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
            if (Mouse.GetState().X > xCord && Mouse.GetState().X < (xCord + width) && Mouse.GetState().Y > yCord && Mouse.GetState().Y < (yCord + height))
            {
                return true;
            }



            return false;
        }



    }
}
