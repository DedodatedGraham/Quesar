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
        public bool isDivided { get; set; }
        public List<Point> points { get; set; }
        public List<string> types { get; set; }

        //So each point will have a type
        public string type { get; set; }
        public Rectangle boundary { get; set; }

        public QuadTree northEast { get; set; }
        public QuadTree southEast { get; set; }
        public QuadTree southWest { get; set; }
        public QuadTree northWest { get; set; }


        public QuadTree(List<Point> pts, List<string> typ, string tp, Rectangle bounds)
        {
            points = new List<Point>();
            type = tp;
            boundary = bounds;
            //Now we go through and add in applicable points into the quad tree if it applies
            applyPoints(pts, typ);
            //sub divide if the tree needs it
            subDivide();
            
        }
        public QuadTree(List<Point> pts,Rectangle bounds)
        {
            points = pts;
            boundary = bounds;
            //sub divide if needed
            subDivide();
        }


        public void update()
        {

        }
        public void subDivide()
        {
            if(points.Count >= 4)
            {
                isDivided = true;
                List<Point> ne = new List<Point>();
                List<Point> se = new List<Point>();
                List<Point> sw = new List<Point>();
                List<Point> nw = new List<Point>();
                for(int i = 0; i< points.Count; i++)
                {
                    if(points[i].X >= boundary.Center.X && points[i].Y <= boundary.Center.Y)
                    {
                        ne.Add(points[i]);
                    }
                    if (points[i].X >= boundary.Center.X && points[i].Y > boundary.Center.Y)
                    {
                        se.Add(points[i]);
                    }
                    if (points[i].X < boundary.Center.X && points[i].Y > boundary.Center.Y)
                    {
                        sw.Add(points[i]);
                    }
                    if (points[i].X < boundary.Center.X && points[i].Y <= boundary.Center.Y)
                    {
                        nw.Add(points[i]);
                    }
                }
                northEast = new QuadTree(ne, new Rectangle(boundary.X + boundary.Width/2,boundary.Y,boundary.Width/2,boundary.Height/2));
                southEast = new QuadTree(se, new Rectangle(boundary.X + boundary.Width / 2, boundary.Y + boundary.Height/2, boundary.Width / 2, boundary.Height / 2));
                southWest = new QuadTree(sw, new Rectangle(boundary.X , boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2));
                northWest = new QuadTree(nw, new Rectangle(boundary.X, boundary.Y , boundary.Width / 2, boundary.Height / 2));
            }

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

       public void Draw(SpriteBatch sb)
        {
            
            sb.DrawRectangle(boundary, Color.Blue);
            if (isDivided)
            {
                northEast.Draw(sb);
                southEast.Draw(sb);
                southWest.Draw(sb);
                northWest.Draw(sb);
            }
            if (!isDivided) {
                for (int i = 0; i < points.Count; i++)
                {
                    sb.DrawPoint(points[i].X, points[i].Y, Color.Black,2);
                }
            }
            

        }
    }

    
}
