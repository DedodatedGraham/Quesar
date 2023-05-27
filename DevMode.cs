using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quesar;

public class DevMode : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    //UpdatePackage
    public UpdatePackage _up;
    //Engine List
    //0 -> UI 
    //1 -> DEV

    //ENGINES
    public List<Engine> _engines;
    public UIEngine _uiEngine;
    public EditEngine _editEngine;
    public SpriteFont defaultFont;
    public DevMode()
    {
        //Start Commands
        Debug.WriteLine("Started");
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _up = new UpdatePackage(_graphics);
        //Assemble Engine Block
        _engines =  new List<Engine>();
        //Create Start Engines
        _uiEngine = new UIEngine();

        //uiengine
        _uiEngine.exitcommand += eExit;
        _uiEngine.startcommand += eStart;
        _uiEngine.fullscreencommand += eFullScreen;
        _uiEngine.editorcommand += eEditor;
        _engines.Add(_uiEngine);

        EngineKill = new List<int>();
        EngineQueue = new List<int>();
    }
    protected override void Initialize()
    {
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.ApplyChanges();
        foreach(Engine e in _engines){
            e.Initialize(_graphics);
        }
        base.Initialize();
    }
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _uiEngine.LoadContent(Content,_graphics);
        defaultFont = Content.Load<SpriteFont>(@"Fonts\DefaultFont");
        
    }   
    protected override void Update(GameTime gameTime)
    {
        _up.keyState = Keyboard.GetState();
        _up.mouseState = Mouse.GetState();
        _up.gameTime = gameTime;
        
        //Mouse Stack
        for(int i = _up.mouseData.Count-1; i >= 1; i--){
            _up.mouseData[i] = _up.mouseData[i - 1];
        }
        if(_up.mouseState.LeftButton == ButtonState.Pressed){
            _up.mouseData[0] = 1;
        }
        else if(_up.mouseState.RightButton == ButtonState.Pressed){
            _up.mouseData[0] = 2;
        }
        else if(_up.mouseState.MiddleButton == ButtonState.Pressed){
            _up.mouseData[0] = 3;
        }
        else{
            _up.mouseData[0] = 0;
        }
        _up._gdm = _graphics;
        foreach(Engine e in _engines){
            e.Update(ref _up);
        }
        //builds & destroys
        EngineDestroy(EngineKill);
        EngineBuild(EngineQueue);
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        foreach(Engine e in _engines){
            e.Draw(_spriteBatch);
        }
        base.Draw(gameTime);
    }


    //Manager Commands
    private List<int> EngineQueue;
    private List<int> EngineKill;
    private void EngineBuild(List<int> add){
        //assumes initalized
        foreach(int i in add){
            switch(i){
                case 0:
                    _engines.Add(_uiEngine);
                    break;
                case 1:
                    _engines.Add(_editEngine);
                    break;
            }
        }
        EngineQueue = new List<int>();
    }
    private void EngineDestroy(List<int> remove){
        //destroy id
        foreach(int i in remove){
            switch(i){
                case 0:
                    _engines.Remove(_uiEngine);
                    break;
                case 1:
                    _engines.Remove(_editEngine);
                    break;
            }
        }
        EngineKill = new List<int>();
    }
    //Outside Commands
    protected void eExit(object sender, EventArgs e){
        //Absoulte exit command, (save and exit)
        Exit();
    }
    protected void eEditor(object sender, EventArgs e){
        //start up editor, Load and run
        _editEngine = new EditEngine();
        _editEngine.defaultFont = defaultFont;
        _editEngine.Initialize(_graphics);
        _editEngine.exitcommand += eExit;
        EngineQueue.Add(1);
    }
    protected void eStart(object sender, EventArgs e){
        //start up game, Load and run
    }
    protected void eFullScreen(object sender, EventArgs e){
        //Adjusts to and from Fullscreen
        if(_graphics.IsFullScreen){
            _graphics.ToggleFullScreen();
            _graphics.ApplyChanges();
        }
        else{
            _graphics.ToggleFullScreen();
            _graphics.ApplyChanges();
        }
    }
}
