using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.ViewportAdapters;
using Windows.Storage;

namespace Quesar
{
    public class Game1 : Game
    {
        public Dictionary dictionary;
        
        public Player thisPlayer;
        public SpriteFont publicFont;

        public bool gameRun;
        public string globalPath = @"C:\Users\graha\source\repos\Quesar\bin\Debug\netcoreapp3.1\Data";


        public int camMovementSpeed;
        public float zooom;

        private bool editActive;
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public int uiStage;
        public UiManager _uiManager;




        public MapEditor editor;



        public Texture2D background;
        public Texture2D altbackground;
        public Texture2D buttonv1;
        public Texture2D charDisplayBox;
        public Texture2D defaulSkin;

        //Camera
        private OrthographicCamera _camera;


        //compnents to making the testShip
        private int lastScroll;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //test ship initializer 
            //testShip = new IronScrapper(_graphics.GraphicsDevice,Content);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            _graphics.ApplyChanges();

            uiStage = 0;

            editActive = false;

            




            base.Initialize();

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _camera = new OrthographicCamera(viewportAdapter);
        }

        protected override void LoadContent()
        {

            
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            


            // TODO: use this.Content to load your game content here

            dictionary = new Dictionary(Content, globalPath);
            background = Content.Load<Texture2D>("BackgroundV2");
            altbackground = Content.Load<Texture2D>("BackgroundV1");
            buttonv1 = Content.Load<Texture2D>("ButtonV1");
            publicFont = Content.Load<SpriteFont>("Gemmu");
            charDisplayBox = Content.Load<Texture2D>("CharacterDisplayBox");
            defaulSkin = Content.Load<Texture2D>("DefaultCharV1");

            _uiManager = new UiManager(GraphicsDevice, buttonv1,charDisplayBox, _graphics,defaulSkin);


            thisPlayer = new Player(_graphics,GraphicsDevice, defaulSkin, "temp");

            editor = new MapEditor(GraphicsDevice, buttonv1, charDisplayBox, _graphics,Content,globalPath);





        }

        protected override void Update(GameTime gameTime)
        {

            //editor update that runs if its active
            if (editActive)
            {
                editor.Update(gameTime,new Vector2());
            }


            //PlayerMovement
            if(thisPlayer.isActive)
            {
                thisPlayer.updatePlayer(Keyboard.GetState());
                _camera.Move(thisPlayer.getDirection(Keyboard.GetState()));
                
            }

            //keeps track & sends ui stage info back and fourth
            //ui will run unless turned off
            if(uiStage != -1)
            {
                uiStage = _uiManager.UpdateManager(gameTime, uiStage);

                if (uiStage == 3)
                {
                    Exit();
                }

                if (uiStage == 22)
                {
                    loadEditor();
                    // turns off ui system
                    uiStage = -1;
                }
            }
            


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            var transformMatrix = _camera.GetViewMatrix();

            _spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied,SamplerState.PointWrap, transformMatrix: transformMatrix);

            //Test object for positioning 
            _spriteBatch.DrawRectangle(new RectangleF(0, 0, 100, 100), Color.Pink, 10);

            //draws ui stage things if active
            if(uiStage != 111 && uiStage != -1)
            {
                _spriteBatch.Draw(getUiStageBackground(), new Vector2(0, 0), Color.White);
                _uiManager.Draw(_spriteBatch, publicFont, uiStage);
            }

            if (editActive)
            {
                editor.Draw(_spriteBatch, publicFont);
            }
            
           


            


            //Heres the ui menu drawing



            _spriteBatch.End();

            if (thisPlayer.isActive)
            {

                thisPlayer.Draw(_spriteBatch);
            }

           
        }

       

        private int GetZoom()
        {
            int a = 0;
            var state = Mouse.GetState();

            if (state.ScrollWheelValue > lastScroll)
            {
                a = 1;
            }
            if (state.ScrollWheelValue < lastScroll)
            {
                a = -1;
            }
            lastScroll = state.ScrollWheelValue;
            return a;
        }

        public Texture2D getUiStageBackground()
        {
            switch (uiStage)
            {
                case 0:
                    return background;

                case 1:
                    return altbackground;
                
                case 2:
                    return altbackground;
                case 11:
                    return altbackground;
                case 21:
                    return altbackground;
                case 111:
                    return altbackground;

                


            }
            return background;



        }
        private void loadEditor()
        {
            
            editActive = true;
        }

        




    }
    
}
