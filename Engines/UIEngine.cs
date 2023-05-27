using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Quesar;

public class UIEngine : Engine{

    public int[] screensize{get;set;}
    public override bool _active {get;}

    //Active UI's
    public List<UITree> ActiveUI{get;set;}
    //UI Trees
    private List<int> LoadQueue;
    private List<int> UnloadQueue;
    public UITree MainMenu;
    public UITree DevMenu;
    public UITree GameMenu;

    //Background Texture
    public int activeBack{get;set;}
    public Texture2D MainMenuTx{get;set;}
    public List<Texture2D> ButtonTx{get;set;}
    public SpriteFont defaultFont{get;set;}
    
    public override event EventHandler exitcommand;
    public event EventHandler editorcommand;
    public event EventHandler startcommand;
    public event EventHandler fullscreencommand;
    public UIEngine(){
        ActiveUI = new List<UITree>();
        LoadQueue = new List<int>();
        UnloadQueue = new List<int>();
        activeBack = 0;
    }
    public override void Initialize(GraphicsDeviceManager gdm){
        screensize = new int[2]{gdm.PreferredBackBufferWidth,gdm.PreferredBackBufferHeight};   
        
    }
    public override void LoadContent(ContentManager content,GraphicsDeviceManager gdm){
            //LOAD TEXTURES FIRST
            MainMenuTx = content.Load<Texture2D>(@"Backgrounds\Title");
            //Loads Buttons States
            ButtonTx = new List<Texture2D>();
            ButtonTx.Add(content.Load<Texture2D>(@"UIParts\Button"));
            //Gets Fonts
            defaultFont = content.Load<SpriteFont>(@"Fonts\DefaultFont");
            LoadTree(new List<int>(){0},gdm);
    }
    public override void Update(ref UpdatePackage up){
            foreach(UITree ui in ActiveUI){
                ui.Update(ref up);
            }
            if(LoadQueue.Count > 0){
                LoadTree(LoadQueue,up._gdm);
                LoadQueue = new List<int>();
            }
            if(UnloadQueue.Count > 0){
                UnloadTree(UnloadQueue);
                UnloadQueue = new List<int>();
            }
            
    }
    public override void Draw(SpriteBatch sb){
        sb.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied,SamplerState.PointClamp);
        switch(activeBack){
            case 0:
                sb.Draw(MainMenuTx,new Rectangle(0,0,screensize[0],screensize[1]),Color.White);
                break;
            case -1:
                Texture2D box = new Texture2D(sb.GraphicsDevice,1,1);
                box.SetData(new[] {Color.White});
                sb.Draw(box,new Rectangle(0,0,screensize[0],screensize[1]),Color.Black);
                break;
        }
        foreach(UITree ui in ActiveUI){
            if (ui.isActive){
                ui.Draw(sb);    
            }
        }
        sb.End();
    }

    public void LoadTree(List<int> ids,GraphicsDeviceManager gdm){
        //We loadin/active wanted UI systems.
        //Allows for suspending UI's for quick recall, i.e seperate menus which can exist at the same time or such
        foreach(int id in ids){
            switch(id){
                case 0:
                    if (MainMenu == null){
                        //Creates an instance of MainMenu if not 'active'/exists
                        //nstage,ncolumns,n,mode(0 cent, 1 raise, 2 slam)
                        //1,7,10,0
                        MainMenu = new UITree(gdm.PreferredBackBufferWidth,gdm.PreferredBackBufferHeight,0,0,2,7,10,0);
                        //INITALIZE TEXTURES && ADD
                        //Mainmenu
                        //Scene 0 Main Menu
                        Button StartButton = new Button(0,0,300,300,"Start");
                        StartButton.Initialize(ButtonTx,defaultFont);
                        MainMenu.AddElement(StartButton,0,0,0);
                        StartButton.lc += eStart;

                        Button OptionButton = new Button(0,0,300,300,"Options");
                        OptionButton.Initialize(ButtonTx,defaultFont);
                        MainMenu.AddElement(OptionButton,0,0,1);
                        OptionButton.lc += eOptions;

                        Button EditorButton = new Button(0,0,300,300,"Editor");
                        EditorButton.Initialize(ButtonTx,defaultFont);
                        MainMenu.AddElement(EditorButton,0,0,2);
                        EditorButton.lc += eEditor;

                        Button ExitButton = new Button(0,0,300,300,"Exit");
                        ExitButton.Initialize(ButtonTx,defaultFont);
                        MainMenu.AddElement(ExitButton,0,6,8);
                        ExitButton.lc += eExit;

                        //Scene 1 Options Menu
                        MainMenu.AddElement(ExitButton,1,6,8);
                        ExitButton.lc += eExit;

                        Button FullScreenButton = new Button(0,0,300,300,"To Windowed");
                        FullScreenButton.Initialize(ButtonTx,defaultFont);
                        MainMenu.AddElement(FullScreenButton,1,0,0);
                        FullScreenButton.lc += eFullScreen;

                        Button BackButton = new Button(0,0,300,300,"Back");
                        BackButton.Initialize(ButtonTx,defaultFont);
                        MainMenu.AddElement(BackButton,1,0,8);
                        BackButton.lc += eBack;

                        MainMenu.isActive = true;
                        MainMenu.actives = 0;
                    }
                    //Load in main Menu
                    ActiveUI.Add(MainMenu);
                    break;
                case 1:
                    if(DevMenu == null){
                        DevMenu = new UITree(gdm.PreferredBackBufferWidth,gdm.PreferredBackBufferHeight,0,0,1,7,10,1);

                        Button ExitButton = new Button(0,0,300,300,"Exit");
                        ExitButton.Initialize(ButtonTx,defaultFont);
                        DevMenu.AddElement(ExitButton,0,6,8);
                        ExitButton.lc += eExit;

                        FPS fpsCounter = new FPS();
                        fpsCounter.Initialize(null,defaultFont);
                        DevMenu.AddElement(fpsCounter,0,0,8);
                        DevMenu.isActive = true;
                    }
                    ActiveUI.Add(DevMenu);
                    break;
                case 2:

                    break;
            }
        }
    } 
    public void UnloadTree(List<int> ids){
        for(int i = 0; i < ids.Count; i ++){
            ActiveUI[i].isActive = false;
            ActiveUI.RemoveAt(ids[i]);
        }
    }

    //Update Commands
    public override void eExit(object sender, EventArgs e){
        //Exits to Game for true close
        rExit(EventArgs.Empty);
    }
    public override void rExit(EventArgs e){
        exitcommand?.Invoke(this,e);
    }
    public void eStart(object sender,EventArgs e){
        //Start button Commands, Goes to game for full Launch
        var up = (UpdatePackage) e;
        LoadQueue.Add(2);
        UnloadQueue.Add(0);
        activeBack = -1;
        rStart(EventArgs.Empty);
    }
    public void eEditor(object sender,EventArgs e){
        //Sets up editor
        var up = (UpdatePackage) e;
        LoadQueue.Add(1);
        UnloadQueue.Add(0);
        activeBack = -1;
        rEditor(EventArgs.Empty);
    }
    public void rEditor(EventArgs e){
        editorcommand?.Invoke(this,e);
    }
    public void rStart(EventArgs e){
        startcommand?.Invoke(this,e);
    }

    public void eOptions(object sender, EventArgs e){
        //Option Button Commands
        MainMenu.actives = 1;
    }
    public void eBack(object sender, EventArgs e){
        //Option Button Commands
        MainMenu.actives = 0;
    }

    private bool efstate = true;
    public void eFullScreen(object sender, EventArgs e){
        //Switch FullScreen and Not
        rFullScreen(EventArgs.Empty);
        if(efstate){
            MainMenu.stages[1][0].elements[0].words = "To Fullscreen";
            efstate = false;
        }
        else{
            MainMenu.stages[1][0].elements[0].words = "To Windowed";
            efstate = true;
        }
        
    }
    public void rFullScreen(EventArgs e){
        fullscreencommand?.Invoke(this,e);
    }
}