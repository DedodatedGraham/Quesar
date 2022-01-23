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
        private int pointMax;
        //So each point will have a type
        public string type { get; set; }
        public Rectangle boundary { get; set; }

        public QuadTree northEast { get; set; }
        public QuadTree southEast { get; set; }
        public QuadTree southWest { get; set; }
        public QuadTree northWest { get; set; }

        public bool hasOver { get; set; }

        public int id { get; set; }

        //tracing variables
        public bool isTracing { get; set; }
        public List<Polygon> traces { get; set; }
        public int traceCount { get; set; }
        //markers for upperlevel reccursion to know if something moved/changed

        private bool hasChanged { get; set; }



        //quad tree works well at maintaining itsself and extending it if needed, null lists suck but most of it should be good
        //still some more things to implement like a quad tree which stores the order of points and is able to draw lines betwee, for best hitbox's and movement this
        //should help move clusters over a quad tree with out having to worry about other points in the same tree
        
        public QuadTree(List<MyPoint> pts, Rectangle bounds,string t)
        {
            id = 0;
            times = 0;
            boundary = bounds;
            type = t;
            pointMax = limit * (times + 1);
            //Now we go through and add in applicable points into the quad tree if it applies
            //sub divide if the tree needs it
            points = sort(pts);
            points = idThese(points);
            subDivide();
        }
        
        //makes blank quad tree
        public QuadTree()
        {
            isStatic = true;
            isDivided = false;
            points = new List<MyPoint>();
            type = "location";
            id = 0;
            times = 0;
            pointMax = limit * (times + 1);
            boundary = new Rectangle(0, 0, 1000, 1000);
        }
        //recursive constructor, prevents stack overflow?
        private QuadTree(List<MyPoint> pts, Rectangle bounds, string t,int time,string name)
        {
            times = time + 1;
            pointMax = limit * (times + 1);
            boundary = bounds;
            type = t;
            //Now we go through and add in applicable points into the quad tree if it applies
            //sub divide if the tree needs it
            points = sort(pts);
            subDivide();
            location = name;
        }


        

        //base initialization functions
        private List<MyPoint> sort(List<MyPoint> pts)
        {
            if (pts.Count != 0)
            {
                if (pts[0].sorted == false)
                {
                    List<MyPoint> a = new List<MyPoint>();
                    for (int i = 0; i < pts.Count; i++)
                    {
                        if (pts[i].type == type || pts[i].type == "")
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
            else
            {
                return new List<MyPoint>();
            }
        }
        private List<MyPoint> idThese(List<MyPoint> pts)
        {
            List<MyPoint> temp = new List<MyPoint>();
            if(!(pts is null))
            {
                for(int i = 0; i < pts.Count; i++)
                {
                    if(pts[i].id == "")
                    {
                        pts[i].id = id.ToString();
                        id++;
                    }
                    else
                    {
                        temp.Add(pts[i]);
                    }
                    
                }
            }
            return temp;
        }
        
        private void subDivide()
        {
            if(!(points is null) && !(points.Count == 0))
            {

                if (points.Count >= pointMax && times < limit)
                {
                    isDivided = true;
                    List<MyPoint> ne = new List<MyPoint>();
                    List<MyPoint> se = new List<MyPoint>();
                    List<MyPoint> sw = new List<MyPoint>();
                    List<MyPoint> nw = new List<MyPoint>();
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (points[i].X >= boundary.Center.X && points[i].Y <= boundary.Center.Y)
                        {
                            ne.Add(points[i]);
                        }
                        else if (points[i].X >= boundary.Center.X && points[i].Y > boundary.Center.Y)
                        {
                            se.Add(points[i]);
                        }
                        else if (points[i].X < boundary.Center.X && points[i].Y > boundary.Center.Y)
                        {
                            sw.Add(points[i]);
                        }
                        else if (points[i].X < boundary.Center.X && points[i].Y <= boundary.Center.Y)
                        {
                            nw.Add(points[i]);
                        }
                    }
                    northEast = new QuadTree(ne, new Rectangle(boundary.X + boundary.Width / 2, boundary.Y, boundary.Width / 2, boundary.Height / 2), type, times, "NorthEast");
                    southEast = new QuadTree(se, new Rectangle(boundary.X + boundary.Width / 2, boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2), type, times, "SouthEast");
                    southWest = new QuadTree(sw, new Rectangle(boundary.X, boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2), type, times, "SouthWest");
                    northWest = new QuadTree(nw, new Rectangle(boundary.X, boundary.Y, boundary.Width / 2, boundary.Height / 2), type, times, "NorthWest");
                    points = new List<MyPoint>();
                }
                else
                {
                    if (times >= limit)
                    {
                        hasOver = true;
                    }
                }
            }
            
        }

        private void unDivide()
        {
            isDivided = false;
            List<MyPoint> thing = new List<MyPoint>();
            if(!(northEast.points is null))
            {
                thing.Concat(northEast.points);
            }
            if (!(northWest.points is null))
            {
                thing.Concat(northWest.points);
            }
            if (!(southEast.points is null))
            {
                thing.Concat(southEast.points);
            }
            if (!(southWest.points is null))
            {
                thing.Concat(southWest.points);
            }
            
            northEast = null;
            northWest = null;
            southEast = null;
            southWest = null;
            
            points = thing; 
        }

        //recursive functions(exclube sub divide from this list because this controlls logic)
        //speciffically apply and remove point will recurssivly add in or remove a point to the propper location

        //these apply points to their needed place
        public void applyPoint(MyPoint pt)
        {
            if(times == 0 && pt.id == "")
            {
                pt.id = id.ToString();
                id++;
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
            else
            {
                if(!(points is null)){
                    points.Add(pt);
                }
                else
                {
                    points = new List<MyPoint>();
                    points.Add(pt);
                }
                
                hasChanged = true;
            }
            
        }
        public void applyPoints(List<MyPoint> pts)
        {
            if(!(pts is null) && pts.Count != 0)
            {
                //id's new elements
                if(times == 0 && pts[0].id == "")
                {
                    for(int i = 0; i < pts.Count; i++)
                    {
                        pts[i].id = id.ToString();
                        id++;
                    }
                }
                //then should be able to do the recursion of dumping off into other sectors
                if (isDivided)
                {
                    hasChanged = true;
                    List<MyPoint> ne = new List<MyPoint>();
                    List<MyPoint> se = new List<MyPoint>();
                    List<MyPoint> sw = new List<MyPoint>();
                    List<MyPoint> nw = new List<MyPoint>();
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (points[i].X >= boundary.Center.X && points[i].Y <= boundary.Center.Y)
                        {
                            ne.Add(points[i]);
                        }
                        else if (points[i].X >= boundary.Center.X && points[i].Y > boundary.Center.Y)
                        {
                            se.Add(points[i]);
                        }
                        else if (points[i].X < boundary.Center.X && points[i].Y > boundary.Center.Y)
                        {
                            sw.Add(points[i]);
                        }
                        else if (points[i].X < boundary.Center.X && points[i].Y <= boundary.Center.Y)
                        {
                            nw.Add(points[i]);
                        }
                    }
                    if(ne.Count != 0)
                    {
                        northEast.applyPoints(ne);
                    }
                    if (nw.Count != 0)
                    {
                        northWest.applyPoints(nw);
                    }
                    if (se.Count != 0)
                    {
                        southEast.applyPoints(se);
                    }
                    if (sw.Count != 0)
                    {
                        southWest.applyPoints(sw);
                    }
                }
                else
                {
                    hasChanged = true;
                    if(!(points is null) && points.Count != 0)
                    {
                        for (int i = 0; i < pts.Count; i++)
                        {
                            points.Add(pts[i]);
                        }
                    }
                    else
                    {
                        if(points.Count == 0)
                        {
                            points = pts;
                        }
                        else
                        {
                            points = new List<MyPoint>();
                            points = pts;
                        }
                    }
                    
                    
                }
                
            }
            
            
                
            
        }

        public void removePoint(MyPoint pt)
        {
            if(!(points is null) && points.Count >= pointMax )
            {
                hasOver = true;
            }
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
            if (!(points is null))
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
            else
            {
                return 0;
            }
            
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
                        subDivide();
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
                    if(getCount() < pointMax && hasOver)
                    {
                        unDivide();
                        hasOver = false;
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
            sb.DrawRectangle(boundary, Color.Blue);

            if (isDivided)
            {
                northEast.Draw(sb);
                southEast.Draw(sb);
                southWest.Draw(sb);
                northWest.Draw(sb);
            }
            if (!isDivided && !(points is null)) {
                for (int i = 0; i < points.Count; i++)
                {
                    sb.DrawPoint(points[i].X, points[i].Y, Color.Black,2);
                }
                
            }
            
            

        }

        public List<MyPoint> gatherNear(MyPoint nw, MyPoint sw, MyPoint ne, MyPoint se)

        {
            List<MyPoint> ret = new List<MyPoint>();
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
        public List<MyPoint> gatherAll()
        {
            //returns all points
            List<MyPoint> ret = new List<MyPoint>();
            if (isDivided)
            {
                List<MyPoint> nwp = northWest.gatherAll();
                for(int i = 0; i < nwp.Count; i++)
                {
                    ret.Add(nwp[i]);
                }
                List<MyPoint> swp = southWest.gatherAll();
                for (int i = 0; i < swp.Count; i++)
                {
                    ret.Add(swp[i]);
                }
                List<MyPoint> nep = northEast.gatherAll();
                for (int i = 0; i < nep.Count; i++)
                {
                    ret.Add(nep[i]);
                }
                List<MyPoint> sep = southEast.gatherAll();
                for (int i = 0; i < sep.Count; i++)
                {
                    ret.Add(sep[i]);
                }
            }
            else if(!(points is null) && points.Count != 0)
            {
                //checks which points are within bounds
                for (int i = 0; i < points.Count; i++)
                {
                    ret.Add(points[i]);
                }
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
                            points[i].rendered = true;
                            ret.Add(points[i]);
                        }

                    }
                    
                }
            }

            return ret;
        }

        //
        //for the traces for hit box registering, we will have this first function which simpily assigns the points its given values of 
        //the trace, order and what not
        public Polygon makeTrace(List<MyPoint> pts,string name)
        {
            List<MyPoint> var = new List<MyPoint>();
            //elements coming in here will never be empty, so null points shouldnt matter
            //also wont ever be making a trace with existing points really, only modifying maybe, 
            //but this trace can be put directly into the add of the list of my points function, savving the trace then adding the points
            for(int i = 0; i < pts.Count; i++)
            {
                pts[i].traceName = name;
                pts[i].inTrace = true;
                pts[i].traceOrder = i;
                var.Add(pts[i]);
            }
            Polygon a = new Polygon(var);
            traces.Add(a);
            return a;
        }

        //may need update loops to make checks and what not
        public void traceUpdate()
        {

        }
        //drawing the trace
        //might change to something recursive if the traces ever become hefty on processing (applying to all traces)
        //the points are also stored in the quad tree its self for total object calulations if needed
        public void DrawTrace(SpriteBatch sb)
        {
            //this works with out recursion because the points are stored in the highest layer making them easily accesable
           
            if(!(traces is null) && traces.Count != 0)
            {
                for (int i = 0; i < traces.Count; i++)
                {
                    traces[i].Draw(sb);
                }

            }
            

        }
        //now i want to be able to input a name of a trace and then get the polyhon of them
        public Polygon getTraceBounderies(string name)
        {
            for(int i = 0; i < traces.Count; i++)
            {
                if(traces[i].lines[0].pt1.traceName == name)
                {
                    return traces[i];
                }
            }
            return new Polygon();
            
            
        }



    }


    public class MyPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string type {get; set;}
        public bool sorted { get; set; }

        public string traceName { get; set; }
        public bool rendered { get; set; }
        public string id { get; set; }
        //trace elements
        public bool inTrace { get; set; }
        public int traceOrder { get; set; }

        public MyPoint(int x, int y, string t)
        {
            X = x;
            Y = y;
            type = t;
            sorted = false;
            id = "";
        }
        public MyPoint()
        {
            X = 0;
            Y = 0;
            type = "";
            sorted = false;
            id = "";
        }

        public bool Equals(MyPoint test)
        {
            if (test.X == X && test.Y == Y)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //these can just return themselves as a different object so making things are easier when the point can cast its self according to the x & y
        public Point toPoint()
        {
            return new Point(X, Y);
        }
        public Vector2 toVector()
        {
            return new Vector2(X, Y);
        }
    }

    //so basically a line contains 2 points, but it will have functions to adjust it, should only be used as a tempory structure, lines should never be saved in quad tree to prevent data build up
    //though i could change that opionion depending on how much itll actually take up
    //could also just change traces to be polygons and write a composition and decomposition function to make it a list of points/ back to polygon
    public class Line
    {
        
        
        public MyPoint pt1 { get; set; }
        public MyPoint pt2 { get; set; }
        //if direction is needed it can be used
        public List<string> direction { get; set; }
        public Line(MyPoint p1, MyPoint p2)
        {
            pt1 = p1;
            pt2 = p2;
            direction = new List<string>();
        }
        public Line()
        {
            pt1 = new MyPoint();
            pt2 = new MyPoint();
            direction = new List<string>();
        }

        public bool Intersects(Line test)
        {
            //this will retermine if either side of this line falls between either side of the test line, 
            //returns true if crossing on either side(pt1 & pt2)
            int dxc1 = pt1.X - test.pt1.X;
            int dyc1 = pt1.Y - test.pt1.Y;
            int dxl1 = test.pt2.X - test.pt1.X;
            int dyl1 = test.pt2.Y - test.pt1.Y;
            int cross1 = dxc1 * dyl1 - dyc1 * dxl1;
            int dxc2 = pt2.X - test.pt1.X;
            int dyc2 = pt2.Y - test.pt1.Y;
            int dxl2 = test.pt2.X - test.pt1.X;
            int dyl2 = test.pt2.Y - test.pt1.Y;
            int cross2 = dxc2 * dyl2 - dyc2 * dxl2;
            if(cross1 != 0 && cross2 != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawLine(pt1.toVector(), pt2.toVector(), Color.Red);
        }
        

    }
    //same goes for the polygons, which are mainly just a list of lines,
    

    //might add in a new empty & not empty variable so if its not empty we can use it to draw textures accoring to any N polygon
    public class Polygon
    {
        public int count;
        public List<Line> lines { get; set; }
        // will get the retalive center point of the polygon for finding the direction of the points 
        public MyPoint center { get; set; }
        public Polygon(List<MyPoint> pts)
        {
            int x = 0;
            int y = 0;
            //only point it stores twice is the orgin
            lines = new List<Line>();
            for (int i = 0; i < pts.Count; i++)
            {
                if (i < pts.Count - 1)
                {
                    lines.Add(new Line(pts[i], pts[i + 1]));
                }
                else
                {
                    lines.Add(new Line(pts[i], pts[0]));
                }
                x += pts[i].X;
                y += pts[i].Y;


            }
            //type center points only exist in polygons for now,
            center = new MyPoint((int)(x / pts.Count), (int)(y / pts.Count), "center");


            //these will just be tags, can contain 4 because 2 points 2 units, can just scan for whatever directions needed
            for (int i = 0; i < lines.Count; i++)
            {
                //the string will basically be pt1 east/west north/south and then pt2 e/w n/s
                if (lines[i].pt1.X >= center.X)
                {
                    lines[i].direction.Add("East");
                }
                else
                {
                    lines[i].direction.Add("West");
                }
                if (lines[i].pt1.Y >= center.Y)
                {
                    lines[i].direction.Add("North");
                }
                else
                {
                    lines[i].direction.Add("South");
                }


                if (lines[i].pt2.X >= center.X)
                {
                    lines[i].direction.Add("East");
                }
                else
                {
                    lines[i].direction.Add("West");
                }
                if (lines[i].pt2.Y >= center.Y)
                {
                    lines[i].direction.Add("North");
                }
                else
                {
                    lines[i].direction.Add("South");

                }

            }

            count = lines.Count;
        }
        public Polygon()
        {
            lines = new List<Line>();
            count = 0;
        }
        public void setPolygon(List<MyPoint> pts)
        {
            int x = 0;
            int y = 0;
            //only point it stores twice is the orgin
            lines = new List<Line>();
            for (int i = 0; i < pts.Count; i++)
            {
                if (i < pts.Count - 1)
                {
                    lines.Add(new Line(pts[i], pts[i + 1]));
                }
                else
                {
                    lines.Add(new Line(pts[i], pts[0]));
                }
                x += pts[i].X;
                y += pts[i].Y;


            }
            //type center points only exist in polygons for now,
            center = new MyPoint((int)(x / pts.Count), (int)(y / pts.Count), "center");


            //these will just be tags, can contain 4 because 2 points 2 units, can just scan for whatever directions needed
            for (int i = 0; i < lines.Count; i++)
            {
                //the string will basically be pt1 east/west north/south and then pt2 e/w n/s
                if (lines[i].pt1.X >= center.X)
                {
                    lines[i].direction.Add("East");
                }
                else
                {
                    lines[i].direction.Add("West");
                }
                if (lines[i].pt1.Y >= center.Y)
                {
                    lines[i].direction.Add("North");
                }
                else
                {
                    lines[i].direction.Add("South");
                }


                if (lines[i].pt2.X >= center.X)
                {
                    lines[i].direction.Add("East");
                }
                else
                {
                    lines[i].direction.Add("West");
                }
                if (lines[i].pt2.Y >= center.Y)
                {
                    lines[i].direction.Add("North");
                }
                else
                {
                    lines[i].direction.Add("South");

                }

            }

            count = lines.Count;
        }

        public bool Intersects(Polygon test)
        {
            //since 2 polygons will have directions of the lines, we only need to check lines that are between the points

            //we then need to find the min and max points of each y

            
            Line tempY = new Line();
            tempY = getMinMaxY();
            Line tempX = new Line();
            tempX = getMinMaxX();

            //broad test case, basically sees if they are even close enough to propperly touch, this will allow for accurate hit detection
            if (tempY.Intersects(test.getMinMaxY()) && tempX.Intersects(test.getMinMaxX()))
            {
                //next we will determine the direction of the other polygon from left or right
                //if this polygon is to the right of the test case
                if (center.X >= test.center.X)
                {
                    List<Line> east = new List<Line>();
                    List<Line> west = new List<Line>();

                    //if its to the right we want to grab all the lines with any tag east and test them with the test cases west tags
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].direction[0] == "East" || lines[i].direction[3] == "East")
                        {
                            east.Add(lines[i]);
                        }
                    }
                    for (int i = 0; i < test.lines.Count; i++)
                    {
                        if (test.lines[i].direction[0] == "West" || test.lines[i].direction[3] == "West")
                        {
                            west.Add(lines[i]);
                        }
                    }
                    //now we have all the lines that correspond, and can then test them all together 

                    for(int i = 0; i < east.Count; i++)
                    {
                        for(int j = 0; j < west.Count; j++)
                        {
                            if (east[i].Intersects(west[j]))
                            {
                                return true;
                            }
                        }
                    }

                }
                //if the polygon is to the left of the test case
                else
                {
                    List<Line> east = new List<Line>();
                    List<Line> west = new List<Line>();

                    //if its to the right we want to grab all the lines with any tag east and test them with the test cases west tags
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].direction[0] == "West" || lines[i].direction[3] == "West")
                        {
                            west.Add(lines[i]);
                        }
                    }
                    for (int i = 0; i < test.lines.Count; i++)
                    {
                        if (test.lines[i].direction[0] == "East" || test.lines[i].direction[3] == "East")
                        {
                            east.Add(lines[i]);
                        }
                    }
                    //now we have all the lines that correspond, and can then test them all together 

                    for (int i = 0; i < east.Count; i++)
                    {
                        for (int j = 0; j < west.Count; j++)
                        {
                            if (east[i].Intersects(west[j]))
                            {
                                return true;
                            }
                        }
                    }

                }

                

            }

            //so it will return true if first, its close enough to where min's and max's intersect, then checks if to left of right, and then goes through boths closest wides

            return false;
        }

        private Line getMinMaxY()
        {
            Line ret = new Line();
            int min = 0;
            int max = 0;
            //this will set min and max to the absolute min and max's of the polygon in the sense of Y
            for(int i = 0; i < lines.Count; i++)
            {
                if(lines[i].pt1.Y > max)
                {
                    max = lines[i].pt1.Y;
                }
                if (lines[i].pt2.Y > max)
                {
                    max = lines[i].pt2.Y;
                }
                if (lines[i].pt1.Y < min)
                {
                    min = lines[i].pt1.Y;
                }
                if (lines[i].pt2.Y < min)
                {
                    min = lines[i].pt2.Y;
                }
            }

            ret.pt1 = new MyPoint(0, max,"max");
            ret.pt2 = new MyPoint(0, min, "min");

            return ret;

        }
        private Line getMinMaxX()
        {
            Line ret = new Line();
            int min = 0;
            int max = 0;
            //this will set min and max to the absolute min and max's of the polygon in the sense of Y
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].pt1.X > max)
                {
                    max = lines[i].pt1.X;
                }
                if (lines[i].pt2.X > max)
                {
                    max = lines[i].pt2.X;
                }
                if (lines[i].pt1.X < min)
                {
                    min = lines[i].pt1.X;
                }
                if (lines[i].pt2.X < min)
                {
                    min = lines[i].pt2.X;
                }
            }

            ret.pt1 = new MyPoint(max, 0, "max");
            ret.pt2 = new MyPoint(min, 0, "min");

            return ret;

        }

        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < lines.Count; i++)
            {
                lines[i].Draw(sb);
            }

        }

        public List<string> getIds()
        {
            List<string> ret = new List<string>();
            for(int i = 0; i < lines.Count; i++)
            {
                ret.Add(lines[i].pt1.id);
            }
            
            return ret;
        }
    }
}
