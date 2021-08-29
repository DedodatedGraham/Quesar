using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quesar
{
    public class PhantomGaze : Ship
    {
        public const int xMaxSize = 55;
        public const int yMaxSize = 85;
        public const int spm = 200000;
        public int sp;
        public const int ac = 100000;
        public int ar;
        public int sh;
        public int si;
        public static ContentManager contentManager;

        public PhantomGaze(GraphicsDevice gd,ContentManager con) : base(gd, spm, 100000, ac, 200, 100, 400,xMaxSize,yMaxSize)
        {
            contentManager = con;
            contentManager.Load<Texture2D>("");
        }
    }
}
