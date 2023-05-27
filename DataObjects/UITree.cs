using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Quesar;



public class UITree{
//UI Tree is the containing class. it understands what elements are where, how they connect
    
    //State Varibales
    public bool isActive{get;set;} = false;

    //UI pos on screen and size
    public int width{get;set;}
    public int height{get;set;}
    public int x{get;set;}
    public int y{get;set;}

    //Numerical values
    public int ns{get;set;}//this n is amount of stages
    public int nc{get;set;}//this n is amount of collumns
    public int n{get;set;}//this n is amount of geometric 'items' per collumn
    public int wc{get;set;}

    //Position values(These Relative to UI)
    private int mw;//Max Width of a col
    private int mh;//Max Heigth of a col

    //Data Containers
    public List<List<UIColumn>> stages{get;set;}
    public int actives{get;set;}//Determines which stages in the UI Column are active. Allows for multiple active in same tree





    public UITree(int w, int h,int x, int y,int ns,int nc,int n,int m){
        this.x = x;
        this.y = y;
        this.width = w;
        this.height = h;
        this.ns = ns;
        this.nc = nc;
        this.n = n;
        this.wc = width / nc;
        stages = new List<List<UIColumn>>(); 
        
        //Next we will define some screen variables for posisitioning
        //Max width and Height for an element
        double tw = this.width / this.nc;
        this.mw = (int)Math.Floor(tw);
        double th = this.height / this.n;
        this.mh = (int)Math.Floor(th);

        for(int i = 0 ; i < ns ; i++){
            stages.Add(new List<UIColumn>());
            for(int j = 0 ; j < nc; j++){
                stages[i].Add(new UIColumn(n,h,mh,m));
            }
        }


    }
    public void AddElement(UIElement element,int stage,int column,int pos,bool overrider = false){
        if(pos<=n-1){
            if(!overrider){
                //Adjust Element To Fix Max if exceeding and position to be correct & doesnt want to be bigger
                if(element.w > mw){
                    element.w = mw;
                }
                if(element.h > mh){
                    element.h = mh;
                }
            }
            //We do however control the centered column for all
            element.x = column * mw + (mw-element.w) / 2;
            //Adds element 
            stages[stage][column].AddElement(element,pos);
        }
    }

    public void Update(ref UpdatePackage up){
        if(up.mouseData.GetRange(1,up.mouseData.Count - 2).IndexOf(1) == -1 && up.mouseData.GetRange(1,up.mouseData.Count - 2).IndexOf(2) == -1){
            bool getout = false;
            foreach(UIColumn col in stages[actives]){
                //Click Detection and sending
                foreach(UIElement ele in col.elements){
                    if(ele.GetType() == typeof(FPS)){
                        //defaults for fps and skips checks
                        ele.Update(ref up);
                    }
                    else if(up.mouseState.X > ele.x && up.mouseState.X < ele.x + ele.w){
                        if(up.mouseState.Y > ele.y && up.mouseState.Y < ele.y + ele.h){
                            ele.Update(ref up);
                            getout = true;
                            break;
                        }
                    }
                }
                if(getout) break;
            }
        }
    }
    public void Draw(SpriteBatch sb){
        //int j = 0;
        foreach(UIColumn col in stages[actives]){
            //Texture2D box = new Texture2D(sb.GraphicsDevice,1,1);
            //box.SetData(new[] {Color.White});
            //sb.Draw(box,new Rectangle((j*mw)+mw/2,0,10,mh*n),Color.White);
            col.Draw(sb);
            //j += 1;
        }   
    }
}