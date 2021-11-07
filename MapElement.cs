using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    abstract public class MapElement
    {
        public abstract bool isActive { get; set; }
        public abstract int tileY { get; set; }
        public abstract int tileX { get; set; }
        public abstract void Draw(SpriteBatch sb);

    }
}
