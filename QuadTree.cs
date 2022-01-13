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
        private bool isDivided { get; set; }
        public List<MyPoint> points { get; set; }

        //recursion limiter
        private const int limit = 10;
        private int times;
        private const int pointMax = 10;
        //So each point will have a type
        public string type { get; set; }
        public Rectangle boundary { get; set; }

        public QuadTree northEast { get; set; }
        public QuadTree southEast { get; set; }
        public QuadTree southWest { get; set; }
        public QuadTree northWest { get; set; }


        //markers for upperlevel reccursion to know if something moved/changed

        private bool hasChanged { get; set; }

        //as of now the quadtree can build its self initially, can use recursion to only store the right kind of points throughout the entire
        //next step is to make the quad tree be able to update & maintain its own form & structure
        public QuadTree(List<MyPoint> pts, Rectangle bounds,string t)
        {
            times = 0;
            boundary = bounds;
            type = t;
            //Now we go through and add in applicable points into the quad tree if it applies
            //sub divide if the tree needs it
            pts = sort(pts);
            subDivide(pts);
        }
        //recursive constructor, prevents stack overflow?
        private QuadTree(List<MyPoint> pts, Rectangle bounds, string t,int time)
        {
            times = time + 1;
            boundary = bounds;
            type = t;
            //Now we go through and add in applicable points into the quad tree if it applies
            //sub divide if the tree needs it
            pts = sort(pts);
            subDivide(pts);
        }


        

        //base initialization functions
        private List<MyPoint> sort(List<MyPoint> pts)
        {
            if (pts[0].sorted == false)
            {
                List<MyPoint> a = new List<MyPoint>();
                for(int i = 0;i< pts.Count; i++)
                {
                    if (pts[i].type == type)
                    {
                        pts[i].sorted = true;
                        a.Add(pts[i]);
                    }
                }
                return a;
            }
            else
            {
                return pts;
            }

        }
        private void addPoints(List<MyPoint> pts)
        {
            for(int i = 0; i < pts.Count; i++)
            {
                points.Add(pts[i]);
            }
        }
        private void subDivide(List<MyPoint> pts)
        {
            if (pts.Count >= pointMax && times < limit)
            {
                isDivided = true;
                List<MyPoint> ne = new List<MyPoint>();
                List<MyPoint> se = new List<MyPoint>();
                List<MyPoint> sw = new List<MyPoint>();
                List<MyPoint> nw = new List<MyPoint>();
                for (int i = 0; i < pts.Count; i++)
                {
                    if (pts[i].X >= boundary.Center.X && pts[i].Y <= boundary.Center.Y)
                    {
                        ne.Add(pts[i]);
                    }
                    if (pts[i].X >= boundary.Center.X && pts[i].Y > boundary.Center.Y)
                    {
                        se.Add(pts[i]);
                    }
                    if (pts[i].X < boundary.Center.X && pts[i].Y > boundary.Center.Y)
                    {
                        sw.Add(pts[i]);
                    }
                    if (pts[i].X < boundary.Center.X && pts[i].Y <= boundary.Center.Y)
                    {
                        nw.Add(pts[i]);
                    }
                }
                northEast = new QuadTree(ne, new Rectangle(boundary.X + boundary.Width / 2, boundary.Y, boundary.Width / 2, boundary.Height / 2), type, times);
                southEast = new QuadTree(se, new Rectangle(boundary.X + boundary.Width / 2, boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2), type, times);
                southWest = new QuadTree(sw, new Rectangle(boundary.X, boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2), type, times);
                northWest = new QuadTree(nw, new Rectangle(boundary.X, boundary.Y, boundary.Width / 2, boundary.Height / 2), type, times);
            }
            else
            {
                addPoints(pts);
            }

        }



        //recursive functions(exclube sub divide from this list because this controlls logic)
        private void applyPoint(MyPoint pt)
        {
            //apply points will spread out and add in points 
            if (isDivided)
            {
                // recurrsively finds the lowest sector to find the lowest point in which it is divided
                if (pt.X >= boundary.Center.X && pt.Y <= boundary.Center.Y)
                {
                    northEast.applyPoint(pt);
                    hasChanged = true;
                }
                if (pt.X >= boundary.Center.X && pt.Y > boundary.Center.Y)
                {
                    southEast.applyPoint(pt);
                    hasChanged = true;
                }
                if (pt.X < boundary.Center.X && pt.Y > boundary.Center.Y)
                {
                    southWest.applyPoint(pt);
                    hasChanged = true;
                }
                if (pt.X < boundary.Center.X && pt.Y <= boundary.Center.Y)
                {
                    northWest.applyPoint(pt);
                    hasChanged = true;
                }
            }
            else{
                points.Add(pt);
                hasChanged = true;
            }
            
        }

        private bool Check()
        {
            if (!isDivided)
            {
                //undivided & changed
                if (hasChanged)
                {
                    //needs to check that max's have not been met 
                    if (points.Count >= pointMax && times < limit)
                    {
                        // we can use sub divide to send out all points into new lower quad trees with the correct divided sequences
                        subDivide(points);
                        //points is set to nothing again because points speciffically  
                        points = new List<MyPoint>();
                        return true;
                    }
                }
            }
            else
            {
                //this will run through check recusivly until the change has been found, and keep track of any reordering that needs to be done
                if (hasChanged)
                {
                    bool needsReorder = Check();



                }
            }
                
           
        }
        private void removePoints(List<MyPoint> pts)
        {
            for(int i = 0; i<pts.Count; i++)
            {
                points.Remove(pts[i]);
            }
        }

        public void update()
        {
            Check();


        }
        public void Draw(SpriteBatch sb)
        {
            
            
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
                sb.DrawRectangle(boundary, Color.Blue);
            }
            

        }
    }

    public class MyPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string type {get; set;}
        public bool sorted { get; set; }
        public MyPoint(int x, int y, string t)
        {
            X = x;
            Y = y;
            type = t;
            sorted = false;
        }
    }

    
}
