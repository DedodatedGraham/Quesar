using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quesar.GameCustomClasses
{
    class Engine
    {

        public int maxSpeed { get; set; }
        public int curSpeed { get; set; }
        public string name  { get; set; }


        public Texture2D texture;

        public Engine(ContentManager con,int mS,int cS,string nm)
        {
            texture = con.Load<Texture2D>("");
            maxSpeed = mS;
            curSpeed = cS;
            name = nm;

        }

        



    }
}
