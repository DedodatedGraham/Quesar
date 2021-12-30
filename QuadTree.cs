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
    public class QuadTree
    {
        //This referes to if the quad tree in Question will be static or no
        public bool isStatic { get; set; }
        public List<Point> points { get; set; }
        public List<string> types { get; set; }

        //So each point will have a type
        public string type { get; set; }
        public Rectangle boundary { get; set; }


        public QuadTree(List<Point> pts, List<string> typ, string tp)
        {
            points = new List<Point>();
            type = tp;
            //Now we go through and add in applicable points into the quad tree if it applies
            applyPoints(pts, typ);

        }
        public QuadTree(List<Point> pts)
        {
            points = pts;
        }

        public void SubDivide()
        {


        }

        public void applyPoints(List<Point> pts, List<string> str)
        {
            for(int i = 0; i< pts.Count; i++)
            {
                if(str[i] == type)
                {
                    //only needs to add points once defined as the correct type in the correct quadtree
                    points.Add(pts[i]);
                }
            }
        }

       
    }

    
}
