using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Quesar;

//Engine is designed to be the main type of thign that is ran, all the active systems of the game will be called through each 
//cycle, Unactives will be ignored
public abstract class Engine{ 
    public abstract event EventHandler exitcommand;
    public abstract bool _active{get;}
    public abstract void Initialize(GraphicsDeviceManager gdm);

    public abstract void LoadContent(ContentManager content,GraphicsDeviceManager gdm);

    public abstract void Update(ref UpdatePackage up);

    public abstract void Draw(SpriteBatch sb);
    public abstract void eExit(object sender, EventArgs e);
    public abstract void rExit(EventArgs e);

}


public class UpdatePackage : EventArgs{
    public GameTime gameTime{get;set;}
    public KeyboardState keyState{get;set;}
    public List<List<string>> keyData{get;set;}
    //Org for key data
    public MouseState mouseState{get;set;}
    public List<int> mouseData{get;set;}
    //Org for mouse data
    //List<int> Current Clicks (1 = left click,2 = right click, 3 = middle click)
    public GraphicsDeviceManager _gdm{get;set;}
    public UpdatePackage(GraphicsDeviceManager gdm){
        keyState = Keyboard.GetState();
        keyData = new List<List<string>>();
        keyData.Add(new List<string>());
        keyData.Add(new List<string>());

        mouseState = Mouse.GetState();
        mouseData = new List<int>();
        //Holds Data for 10 ticks
        for(int i = 0; i < 6; i ++){
            mouseData.Add(0);
        }

        _gdm = gdm;
    }
}