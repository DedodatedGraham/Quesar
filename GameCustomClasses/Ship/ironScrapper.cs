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
    public class IronScrapper : Ship
    {
        
        public const int xMaxSize = 12;
        public const int yMaxSize = 10;
        public const int spm = 2000;
        public int sp;
        public const int ac = 10;
        public int ar;
        public int sh;
        public int si;
        public static ContentManager contentManager;
        


        public IronScrapper(GraphicsDevice gd, ContentManager con) : base(gd,spm,1000,ac,20,10,40,xMaxSize,yMaxSize)
        {
            contentManager = con;
             base.shiptexture = contentManager.Load<Texture2D>("IronScrapperV1");
           
        }
    }
}
