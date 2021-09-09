using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    abstract public class UiElement
    {

        
        public abstract bool isActive { get; set; }
        public abstract void Draw(SpriteBatch sb,SpriteFont sf);

        public abstract bool isClicked();



    }
}
