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

        public override bool state { get; set; }
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

            state = false;


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

                //Some bullshit gotta get drawstring aligned on a pixel or else bad things D:
                sp.Draw(buttonSkin, new Rectangle(xCord, yCord, width, height), Color.White);
                sp.DrawRectangle(r, Color.White);

                double x1 = xCord + (width / 2 - size.X / 2);
                double y1 = yCord + (height / 10);
                x1 = Math.Floor(x1);
                y1 = Math.Floor(y1);
                Point p1 = new Point((int)x1, (int)y1);

                sp.DrawString(font, words,p1.ToVector2(), Color.Black,0,new Vector2(0),1,0,1);
                
                
                double x2 = xCord + (width / 2 - size2.X / 2);
                double y2 = yCord + (height * (55 / 100) + size2.Y);
                x2 = Math.Floor(x2);
                y2 = Math.Floor(y2);
                Point p2 = new Point((int)x2, (int)y2);
                sp.DrawString(font, typed, p2.ToVector2(), Color.Black, 0, new Vector2(0), 1, 0, 1);





            }

        }


        public override bool isClicked()
        {
            

            if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (isHovering()) )
            {
                return true;
            }


            return false;

        }

        public override bool isHovering()
        {
            if (Mouse.GetState().X > xCord && Mouse.GetState().X < (xCord + width) && Mouse.GetState().Y > yCord && Mouse.GetState().Y < (yCord + height))
            {
                return true;
            }



            return false;
        }



    }
}
