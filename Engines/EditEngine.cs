using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quesar;

//Engine is designed to be the main type of thign that is ran, all the active systems of the game will be called through each 
//cycle, Unactives will be ignored
public class EditEngine:Engine{ 
    public int[] screensize{get;set;}
    public override event EventHandler exitcommand;
    public override bool _active{get;}
    //Camera Settings
    public EditCamera devCam{get;set;}
    public bool camReady{get;set;}
    //
    public MapManager mapManager;
    public SpriteFont defaultFont;
    public EditEngine(){
        
    }
    public override void Initialize(GraphicsDeviceManager gdm){
        screensize = new int[2]{gdm.PreferredBackBufferWidth,gdm.PreferredBackBufferHeight}; 
        devCam = new EditCamera(screensize);
        mapManager = new MapManager();
        mapManager.Initialize(defaultFont,gdm);
        //mapManager.GenerateNow();
        camReady = true;
    }

    public override void LoadContent(ContentManager content,GraphicsDeviceManager gdm){
        
    }

    
    private float xdir = 0;
    private float ydir = 0;
    public override void Update(ref UpdatePackage up){
        mapManager.Update(ref up);
        if(camReady){
            if(up.keyState.IsKeyDown(Keys.W)){
                ydir = -1;
            }
            else if(up.keyState.IsKeyDown(Keys.S)){
                ydir = 1;
            }

            if(up.keyState.IsKeyDown(Keys.D)){
                xdir = 1;
            }
            else if(up.keyState.IsKeyDown(Keys.A)){
                xdir = -1;
            }
            if(xdir != 0 || ydir != 0){
                Vector2 newvec = new Vector2(xdir,ydir);
                devCam.MoveCamera(newvec);
                xdir = 0;
                ydir = 0; 
            } 
        }
    }

    public override void Draw(SpriteBatch sb){
        if (camReady){
            sb.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied,SamplerState.PointClamp,transformMatrix: devCam.TranslationMatrix);
        }
        else{
            sb.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied,SamplerState.PointClamp);
        }
        mapManager.Draw(sb);
        sb.End();
    }
    public override void eExit(object sender, EventArgs e){
        //Exits to Game for true close
        rExit(EventArgs.Empty);
    }
    public override void rExit(EventArgs e){
        exitcommand?.Invoke(this,e);
    }
}