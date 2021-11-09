using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Quesar.GameCustomClasses;
using System;

namespace Quesar
{
    public class Game1 : Game
    {
        public Player thisPlayer;
        public SpriteFont publicFont;

        public bool gameRun;


        public int camMovementSpeed;
        public float zooom;
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public int uiStage;
        public UiManager _uiManager;

        public Map gameMap;


        private Texture2D background;
        private Texture2D altbackground;
        public Texture2D buttonv1;
        public Texture2D charDisplayBox;
        public Texture2D defaulSkin;

        public OrthographicCamera camera;

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
            
            

            base.Initialize();

            
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            

            // TODO: use this.Content to load your game content here
            
            
            background = Content.Load<Texture2D>("BackgroundV2");
            altbackground = Content.Load<Texture2D>("BackgroundV1");
            buttonv1 = Content.Load<Texture2D>("ButtonV1");
            publicFont = Content.Load<SpriteFont>("Gemmu");
            charDisplayBox = Content.Load<Texture2D>("CharacterDisplayBox");
            defaulSkin = Content.Load<Texture2D>("DefaultCharV1");

            _uiManager = new UiManager(GraphicsDevice, buttonv1,charDisplayBox, _graphics,defaulSkin);
            gameMap = new Map(GraphicsDevice,100,100,"gameMap",Content);


            thisPlayer = new Player(_graphics,GraphicsDevice, defaulSkin, "temp");

            camera = new OrthographicCamera(GraphicsDevice);
            // Inform Myra that external text input is available
            // So it stops translating Keys to chars



        }

        protected override void Update(GameTime gameTime)
        {
            if (uiStage == 3)
                Exit();

            // TODO: Add your update logic here

            //This updates Camera movement to the testship.
            camMovementSpeed = 2;
            zooom = 0.2f;


            //PlayerMovement
            if(uiStage == 111)
            {
                int con = 32;
                Vector2 move = GetMovementDirection();
                camera.Move(move * camMovementSpeed);
                thisPlayer.hitbox.posTileX += (int)move.X * camMovementSpeed;
                thisPlayer.hitbox.posTileY += (int)move.Y * camMovementSpeed;
                thisPlayer.x += (int)move.X * camMovementSpeed;
                thisPlayer.y += (int)move.Y * camMovementSpeed;
                if (thisPlayer.hitbox.posTileX >= con)
                {
                    thisPlayer.hitbox.tileX++;
                    thisPlayer.hitbox.posTileX = 0;
                }
                if (thisPlayer.hitbox.posTileY >= con)
                {
                    thisPlayer.hitbox.tileY++;
                    thisPlayer.hitbox.posTileY = 0;
                }
                if (thisPlayer.hitbox.posTileX < 0)
                {
                    thisPlayer.hitbox.tileX--;
                    thisPlayer.hitbox.posTileX = con;
                }
                if (thisPlayer.hitbox.posTileY < 0)
                {
                    thisPlayer.hitbox.tileY--;
                    thisPlayer.hitbox.posTileY = con;
                }
                
                    
                
                
                
            }

            thisPlayer.updatePlayer();
            //ui stage logic
           switch(_uiManager.UpdateManager(gameTime, uiStage))
            {
                case 0:
                    uiStage = 0;
                    break;
                case 1:
                    uiStage = 1;
                    break;
                case 11:
                    uiStage = 11;
                    break;
                case 111:
                    gameRun = true;
                    thisPlayer.name = _uiManager.outputNewPlayer;
                    uiStage = 111;
                    //Make sure to update this for getting a players location and spawning in last map
                    gameMap.mapStage = thisPlayer.world;
                    thisPlayer.isActive = true;
                    break;

                case 2:
                    uiStage = 2;
                    break;

                case 3:
                    uiStage = 3;
                    break;

            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied,SamplerState.PointWrap, transformMatrix: camera.GetViewMatrix());
            

            //This is the background and always the last thing on the screen, 

            if(uiStage != 111)
            {
                _spriteBatch.Draw(getUiStageBackground(), new Vector2(0, 0), Color.White);
            }
            
           
            
            _uiManager.Draw(_spriteBatch, publicFont, uiStage);


            gameMap.Draw(_spriteBatch);


            //Heres the ui menu drawing


            _spriteBatch.End();
            if (thisPlayer.isActive)
            {

                thisPlayer.Draw(_spriteBatch);
            }
        }

        private Vector2 GetMovementDirection()
        {
            int p = 0;
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();
            GameCustomClasses.Hitbox[] hb = gameMap.activeHits();
            if (IsColiding(thisPlayer.hitbox,hb, out p) == false)
            {
                
                if (state.IsKeyDown(Keys.S))
                {
                    movementDirection += Vector2.UnitY;
                }
                if (state.IsKeyDown(Keys.W))
                {
                    movementDirection -= Vector2.UnitY;
                }
                if (state.IsKeyDown(Keys.A))
                {
                    movementDirection -= Vector2.UnitX;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    movementDirection += Vector2.UnitX;
                }
            }
            else
            {
                if (thisPlayer.hitbox.GetCorners()[0].X <= hb[p].GetCorners()[1].X)
                {
                    movementDirection = Vector2.UnitX;
                }
                if (thisPlayer.hitbox.GetCorners()[1].X >= hb[p].GetCorners()[0].X)
                {
                    movementDirection = Vector2.UnitX;
                }
                if (thisPlayer.hitbox.GetCorners()[0].Y <= hb[p].GetCorners()[1].Y)
                {
                    movementDirection = Vector2.UnitY;
                }
                if (thisPlayer.hitbox.GetCorners()[1].Y >= hb[p].GetCorners()[0].Y)
                {
                    movementDirection = Vector2.UnitY;
                }


            }
            return movementDirection;
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
                case 111:
                    return altbackground;


            }
            return background;



        }
        
        public bool IsColiding(GameCustomClasses.Hitbox a, GameCustomClasses.Hitbox[] b,out int p)
        {
            bool isc = false;
            //remeber P 1 is for spot 0 in array,
            //did this so if outputs a 0 then no intersection and if outputs intersection it gives specfic cord output aswell as a boolean which can determine
            //if it can move, and if it cant adjusts by some degree of p  
            //saves memory? idno
            p = 0;

            for(int i = 1; i < b.Length; i++)
            {
                if ((a.GetCorners()[0].X <= b[i].GetCorners()[1].X && a.GetCorners()[0].X >= b[i].GetCorners()[0].X) || (a.GetCorners()[1].X <= b[i].GetCorners()[1].X && a.GetCorners()[1].X >= b[i].GetCorners()[0].X))
                {
                    if ((a.GetCorners()[0].Y <= b[i].GetCorners()[1].Y && a.GetCorners()[0].Y >= b[i].GetCorners()[1].Y) || (a.GetCorners()[1].Y <= b[i].GetCorners()[1].Y && a.GetCorners()[1].Y >= b[i].GetCorners()[1].Y))
                    {

                        isc = true;
                        p = i;

                    }

                }
            

            }


            return isc;
        }


    }
    
}
