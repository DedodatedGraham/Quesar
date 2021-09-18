using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    public class PlayerBox : UiElement
    {
        public int xCord { get; set; }
        public int yCord { get; set; }
        public int width { get; set; }
        public int height { get; set; }


        public override bool isActive { get; set; }

        public GraphicsDevice _graphcis;
        public Texture2D BackSkin { get; set; }

        public PlayerBox(GraphicsDevice gd, int x, int y, int w, int h, Texture2D skin, bool isA)
        {
            _graphcis = gd;
            xCord = x;
            yCord = y;
            width = w;
            height = h;
            BackSkin = skin;
            isActive = isA;
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
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
