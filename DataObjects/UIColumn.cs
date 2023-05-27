using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quesar;

public class UIColumn{
    public int n {get;set;}//This is the amount of elements in a given column
    public int height{get;set;}//Total heigth
     public int maxheight{get;set;}//Max heigth of an element(Formated to amount of elements already so wont excede)
    public int mode{get;set;}

    public List<UIElement> elements{get;set;}
    //Mode == 0(Default); Center Column; i0 = cent, i1 = top, i2 = bot, i3 = top
    //Mode == 1; Raise Column, i0 = top
    //Mode == 2; Slam Column, i0 = bot
    public UIColumn(int n,int h,int mh,int m){
        this.n = n;
        this.height = h;
        this.maxheight = mh;
        this.mode = m;
        elements = new List<UIElement>();
    }

    public void AddElement(UIElement element, int pos){
        int count = elements.Count;
        if(mode == 0){
            if(pos != 0){
                int i = pos % 2;
                if(i == 0){
                    //Stacks ontop
                    double newy = height/2 - (pos / 2) * maxheight - maxheight / 2 + (maxheight - element.h);
                    element.y = (int)Math.Floor(newy);
                }
                else{
                    //Stacks onbot
                    double newy = height/2 + ((pos + 1) / 2) * maxheight - maxheight / 2 + (maxheight - element.h);
                    element.y = (int)Math.Floor(newy);
                }
            }
            else{
                double newy = height/2 - maxheight/2 + (maxheight - element.h);
                element.y = (int)Math.Floor(newy);
            }
        }
        element.resize();
        if(count >= n){
            Debug.WriteLine("Too many Elements in col");
        }
        else if(pos > count){
            elements.Add(element);
        }
        else{
            elements.Insert(pos,element);
        }
    }
    public void Draw(SpriteBatch sb){
        foreach(UIElement uiele in elements){
            uiele.Draw(sb);
        }
    }
}