using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    public class ToggleButton : Button
    {

        
        public bool isOn;
        public ToggleButton(GraphicsDevice gd, int x, int y, int w, int h, string word, Texture2D skin, bool isA) :base(gd,x,y,w,h,word,skin,isA)
        {
            isOn = false;
        }


        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            Vector2 size = font.MeasureString(words);

            if (isActive)
            {
                if (!isOn)
                {
                    sp.Draw(buttonSkin, new Rectangle(xCord, yCord, width, height), new Color(Color.White,150));
                    //might need to fiddle with positioning of text later butthis will work for now
                    double x = xCord + (width / 2 - size.X / 2);
                    double y = yCord + (height / 2 - size.Y / 2);
                    x = Math.Floor(x);
                    y = Math.Floor(y);
                    Point p = new Point((int)x, (int)y);

                    sp.DrawString(font, words, p.ToVector2(), Color.Black);
                }
                else
                {
                    sp.Draw(buttonSkin, new Rectangle(xCord, yCord, width, height), Color.White);
                    //might need to fiddle with positioning of text later butthis will work for now
                    double x = xCord + (width / 2 - size.X / 2);
                    double y = yCord + (height / 2 - size.Y / 2);
                    x = Math.Floor(x);
                    y = Math.Floor(y);
                    Point p = new Point((int)x, (int)y);

                    sp.DrawString(font, words, p.ToVector2(), Color.Black);
                }
                
            }
        }
        public override bool isClicked()
        {
            //method for only letting things be pressed & relased and firing once
            
            if(Mouse.GetState().LeftButton == ButtonState.Released)
            {
                state = false;
            }
            if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && isHovering() && !state)
            {
                if (isOn)
                {
                    isOn = false;
                    state = true;
                    return true;
                }
                else
                {
                    isOn = true;
                    state = true;
                    return true;
                }
            }


            return false;

        }

    }
}
