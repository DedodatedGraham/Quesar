using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar.GameCustomClasses
{
    public class Building : MapElement
    {

        public Texture2D skin { get; set; }

        public string name { get; set; }

        public int tileX { get; set; }
        public int tileY { get; set; }

        public Building(Texture2D sk,int x,int y){

            skin = sk;


            tileX = x;
            tileY = y;





        }
    }
}