using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace Quesar;

public class GameMode : Game
{
    public UpdatePackage _up;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private List<Engine> _engines;
    public GameMode()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        _up = new UpdatePackage(_graphics);

        //Make wanted Engine Components
        _engines =  new List<Engine>();
    }

    protected override void Initialize()
    {
        foreach(Engine e in _engines){
            e.Initialize(_graphics);
        }
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach(Engine e in _engines){
            e.LoadContent(Content,_graphics);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        _up.keyState = Keyboard.GetState();
        _up.mouseState = Mouse.GetState();
        foreach(Engine e in _engines){
                    e.Update(ref _up);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        foreach(Engine e in _engines){
            e.Draw(_spriteBatch);
        }

        base.Draw(gameTime);
    }
}
