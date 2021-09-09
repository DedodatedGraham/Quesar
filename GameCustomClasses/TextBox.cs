using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar.GameCustomClasses.Ship
{
    class TextBox : UiElement
    {
        public int xCord { get; set; }
        public int yCord { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public string words { get; set; }

        //for determining if the button is on screen
        public bool isActive { get; set; }

        public GraphicsDevice _graphcis;
        public Texture2D buttonSkin { get; set; }

        public TextBox(GraphicsDevice gd, int x, int y, int w, int h,string word,Texture2D skin, bool isA)
        {
            xCord = x;
            yCord = y;
            width = w;
            height = h;

            isActive = isA;

            buttonSkin = skin;
            _graphcis = gd;

            words = word;



        }


        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            if (isActive)
            {

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
