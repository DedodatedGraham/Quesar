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

        public override bool state { get; set; }

        public override bool isActive { get; set; }

        public GraphicsDevice _graphcis;
        public Texture2D BackSkin { get; set; }
        public Texture2D playerSkin { get; set; }

        public PlayerBox(GraphicsDevice gd, int x, int y, int w, int h, Texture2D border,Texture2D skin, bool isA)
        {
            _graphcis = gd;
            xCord = x;
            yCord = y;
            width = w;
            height = h;
            BackSkin = border;
            playerSkin = skin;
            isActive = isA;
            state = false;
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            //THIS IS DESIGNED TO REALLY ONLY FIT A 300X300 BOX may need to have smaller variant or updated for that depending on if its needed later
            if (isActive)
            {
                sb.Draw(BackSkin, new Rectangle(xCord, yCord, width, height),Color.White);
                sb.Draw(playerSkin, new Rectangle(xCord + width / 2 - 64, yCord + height / 2 - 128, 128, 256),Color.White);
            }



        }

        public override bool isClicked()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                state = false;
            }
            if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (isHovering()) && !state)
            {
                state = true;
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
