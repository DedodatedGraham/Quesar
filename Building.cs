using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Quesar
{
    public class Building : MapElement
    {
        public override string id { get; set; }
        public override string type { get; set; }
        public override List<QuadTree> pointData { get; set; }
        public override Texture2D skin { get; set; }
        public override MyPoint location { get; set; }
        public override bool isActive { get; set; }
        public override bool hasInside { get; set; }
        public Rectangle boundary { get; set; }


        //should be able to load a new building, 
        public Building(Texture2D sk,Rectangle bon,bool hasint,List<QuadTree> bruh){
            pointData = new List<QuadTree>();
            skin = sk;
                boundary = bon;
            
            //if needs interrior makes a door layer-
            hasInside = hasint;
            
            
                
            
        }



        public override void Draw(SpriteBatch sp)
        {
            if (isActive)
            {
                sp.Draw(skin, boundary, Color.White);
            }

        }
    }
}