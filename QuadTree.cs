using System;
using System.Collections.Generic;
using System.Linq;
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
        private string location { get; set; }

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
            if (pts[0].id == "")
            {
                for(int i = 0; i < pts.Count; i++)
                {
                    pts[i].id = i.ToString();
                }
            }
            times = 0;
            boundary = bounds;
            type = t;
            //Now we go through and add in applicable points into the quad tree if it applies
            //sub divide if the tree needs it
            pts = sort(pts);
            subDivide(pts);
        }
        //recursive constructor, prevents stack overflow?
        private QuadTree(List<MyPoint> pts, Rectangle bounds, string t,int time,string name)
        {
            times = time + 1;
            boundary = bounds;
            type = t;
            //Now we go through and add in applicable points into the quad tree if it applies
            //sub divide if the tree needs it
            pts = sort(pts);
            subDivide(pts);
            location = name;
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

        private void removePoints(List<MyPoint> pts)
        {
            for (int i = 0; i < pts.Count; i++)
            {
                points.Remove(pts[i]);
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
                northEast = new QuadTree(ne, new Rectangle(boundary.X + boundary.Width / 2, boundary.Y, boundary.Width / 2, boundary.Height / 2), type, times,"NorthEast");
                southEast = new QuadTree(se, new Rectangle(boundary.X + boundary.Width / 2, boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2), type, times,"SouthEast");
                southWest = new QuadTree(sw, new Rectangle(boundary.X, boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2), type, times,"SouthWest");
                northWest = new QuadTree(nw, new Rectangle(boundary.X, boundary.Y, boundary.Width / 2, boundary.Height / 2), type, times,"NorthWest");
            }
            else
            {
                addPoints(pts);
            }

        }

        private void unDivide()
        {
            isDivided = false;
            List<MyPoint> thing = new List<MyPoint>();
            thing.Concat(northEast.points);
            thing.Concat(northWest.points);
            thing.Concat(southEast.points);
            thing.Concat(southWest.points);
            northEast = null;
            northWest = null;
            southEast = null;
            southWest = null;
            points = thing; 
        }

        //recursive functions(exclube sub divide from this list because this controlls logic)
        //speciffically apply and remove point will recurssivly add in or remove a point to the propper location
        private void applyPoint(MyPoint pt)
        {
            if(times == 0)
            {
                pt.id = getCount().ToString();
            }
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

        private void removePoint(MyPoint pt)
        {
            if (isDivided)
            {
                if (pt.X >= boundary.Center.X && pt.Y <= boundary.Center.Y)
                {
                    northEast.removePoint(pt);
                    hasChanged = true;
                }
                if (pt.X >= boundary.Center.X && pt.Y > boundary.Center.Y)
                {
                    southEast.removePoint(pt);
                    hasChanged = true;
                }
                if (pt.X < boundary.Center.X && pt.Y > boundary.Center.Y)
                {
                    southWest.removePoint(pt);
                    hasChanged = true;
                }
                if (pt.X < boundary.Center.X && pt.Y <= boundary.Center.Y)
                {
                    northWest.removePoint(pt);
                    hasChanged = true;
                }

            }
            else
            {
                for(int i = 0; i < points.Count; i++)
                {
                    if(pt.X == points[i].X && pt.Y == points[i].Y)
                    {
                        points.RemoveAt(i);
                    }
                }

                hasChanged = true;
            }

        }

        public int getCount()
        {
            int count = 0;
            if (isDivided)
            {
                count += northEast.getCount();
                count += northWest.getCount();
                count += southEast.getCount();
                count += southWest.getCount();
            }
            else
            {
                count = count + points.Count;
            }
            return count;
        }
        public List<string> indexBounds(MyPoint pt)
        {
            if (isDivided)
            {
                if (pt.X >= boundary.Center.X && pt.Y <= boundary.Center.Y)
                {
                    List<string> str = new List<string>();
                    str.Add(location);
                    str = (List<string>)str.Concat(northEast.indexBounds(pt));
                    return str;
                }
                if (pt.X >= boundary.Center.X && pt.Y > boundary.Center.Y)
                {
                    List<string> str = new List<string>();
                    str.Add(location);
                    str = (List<string>)str.Concat(southEast.indexBounds(pt));
                    return str;
                }
                if (pt.X < boundary.Center.X && pt.Y > boundary.Center.Y)
                {
                    List<string> str = new List<string>();
                    str.Add(location);
                    str = (List<string>)str.Concat(southWest.indexBounds(pt));
                    return str;
                }
                if (pt.X < boundary.Center.X && pt.Y <= boundary.Center.Y)
                {
                    List<string> str = new List<string>();
                    str.Add(location);
                    str = (List<string>)str.Concat(northWest.indexBounds(pt));
                    return str;
                }

            }
            else
            {
                List<string> str = new List<string>();
                str.Add(location);
                return str;

            }

            return new List<string>(); ;

        }
        public int depthBounds(int x, int y)
        {
            if (isDivided)
            {
                if (x >= boundary.Center.X && y <= boundary.Center.Y)
                {
                    return northEast.depthBounds(x,y);
                }
                if (x >= boundary.Center.X && y > boundary.Center.Y)
                {
                    return southEast.depthBounds(x, y);
                }
                if (x < boundary.Center.X && y > boundary.Center.Y)
                {
                    return southWest.depthBounds(x, y);
                }
                if (x < boundary.Center.X && y <= boundary.Center.Y)
                {
                    return northWest.depthBounds(x, y);
                }

            }
            else if(!isDivided)
            {
                return times;
            }
            return 0;
        }

        public int indexPointNum(MyPoint pt,List<string> bounds)
        {
            int index = 0;
            if (bounds.Count > times)
            {
                if (bounds[times] == northEast.location)
                {
                    return northEast.indexPointNum(pt,bounds);
                }
                if (bounds[times] == northWest.location)
                {
                    return northWest.indexPointNum(pt, bounds);
                }
                if (bounds[times] == southEast.location)
                {
                    return southEast.indexPointNum(pt, bounds);
                }
                if (bounds[times] == southWest.location)
                {
                    return southWest.indexPointNum(pt, bounds);
                }
            }

            else
            {
                for(int i = 0; i< points.Count; i++)
                {
                    if (pt == points[i])
                    {
                        index++;
                        break;
                    }
                    index++;
                }
                
            }

            return index;
          
        }
        //check will go through and divide if needed, and reduce spaces, basically it re orderes when we want it to, update will handle mosta
        private void check()
        {
            //first we will add in and remove the quad tree stuff, make sure it is all ordered properly
            if (!isDivided)
            {
                //Divides into new sectors
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
                    }
                }
            }
            else
            {
                //if divided & changed it needds to make sure it doesnt need to undivide aswell aka if the points fall u8nder the limit it should undo, and it should only have to do that once
                if (hasChanged)
                {
                    if(getCount() < pointMax)
                    {
                        unDivide();
                    }

                }
            }
           
        }

        //ultimate update that should be ran on every quad tree for upkeep with dynamic data points
        public void update()
        {
            //logic for how to update 
            //checks if its divided, goes recursively so undivided logic always happens first
            //prevents us from trying to delete things then trying to update
            //so quad tree run in update of whatever it is in and it will automatically adjust its self when
            //points are added or removed
            if (isDivided)
            {
                northEast.update();
                northWest.update();
                southWest.update();
                southEast.update();

                if (hasChanged)
                {
                    //check after so if it deletes in the check it can still do things
                    check();

                    hasChanged = false;
                }
            }
            else
            {
                if (hasChanged)
                {
                    check();
                    hasChanged = false;
                }

            }

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

        public List<MyPoint> gatherNear(MyPoint nw, MyPoint sw, MyPoint ne, MyPoint se)
        {
            List<MyPoint> ret = new List<MyPoint>();
            int nwd = depthBounds(nw.X, nw.Y);
            int swd = depthBounds(sw.X, sw.Y);
            int ned = depthBounds(ne.X, ne.Y);
            int sed = depthBounds(se.X, se.Y);
            if (isDivided)
            {
                //the program will first run through & see if it can decrease quality from top because rendered in such small area 
                if (se.X < boundary.Center.X && se.Y < boundary.Center.Y)
                {
                    ret = northWest.gatherNear(nw, sw, ne, se);
                }
                else if (sw.X >= boundary.Center.X && sw.Y < boundary.Center.Y)
                {
                    ret = northEast.gatherNear(nw, sw, ne, se);
                }
                else if (ne.X < boundary.Center.X && ne.Y < boundary.Center.Y)
                {
                    ret = southWest.gatherNear(nw, sw, ne, se);
                }
                else if (nw.X >= boundary.Center.X && nw.Y < boundary.Center.Y)
                {
                    ret = southEast.gatherNear(nw, sw, ne, se);
                }
                else
                {
                    ret = gather(nw,se);
                }


                //now it will assume its detailed enough to start gathering points the best

                
            }
            else
            {
                //returns points when the render box is within a lowest box, still scans x & y for locations
                ret = points;


            }

            return ret;
        }

        private List<MyPoint> gather(MyPoint nw, MyPoint se)
        {
            List<MyPoint> ret = new List<MyPoint>();
            if (isDivided)
            {
                ret.Concat(northEast.gather(nw,se));
                ret.Concat(southEast.gather(nw, se));
                ret.Concat(northWest.gather(nw, se));
                ret.Concat(southWest.gather(nw, se));
            }
            else
            {
                //checks which points are within bounds
                for(int i = 0; i < points.Count; i++)
                {
                    if(points[i].X >= nw.X && points[i].X <= se.X)
                    {
                        if (points[i].Y >= nw.Y && points[i].Y <= se.Y)
                        {
                            ret.Add(points[i]);
                        }

                    }
                    
                }
            }

            return ret;
        }


    }


    public class MyPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string type {get; set;}
        public bool sorted { get; set; }

        public string id { get; set; }
        public MyPoint(int x, int y, string t)
        {
            X = x;
            Y = y;
            type = t;
            sorted = false;
            id = "";
        }
    }

    
}
