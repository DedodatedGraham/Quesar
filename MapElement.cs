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
        //It will have an id for relating it to a point, will be determined with 
        public abstract string id { get; set; }
        public abstract string type { get; set; }
        public abstract List<QuadTree> pointData { get; set; }
        public abstract Texture2D skin { get; set; }
        public abstract MyPoint location { get; set; }
        public abstract bool hasInside { get; set; }
        public abstract void Draw(SpriteBatch sb);

    }
}
