using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

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
        

        //compnents to making the testShip
        //private Ship testShip;
        //private int camMovementSpeed;
        //private float zooom;
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


            thisPlayer = new Player(GraphicsDevice, defaulSkin, "temp");
            // Inform Myra that external text input is available
            // So it stops translating Keys to chars



        }

        protected override void Update(GameTime gameTime)
        {
            if (uiStage == 3)
                Exit();

            // TODO: Add your update logic here

            //This updates Camera movement to the testship
            camMovementSpeed = 2;
            zooom = 0.2f;
            if(uiStage == 111)
            {
                thisPlayer.cam.Move(GetMovementDirection() * camMovementSpeed);
                thisPlayer.Move(GetMovementDirection());
            }
            //testShip.camera.Move(GetMovementDirection() * camMovementSpeed );
            //testShip.camera.ZoomIn(GetZoom() * zooom);



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
            _spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied,SamplerState.PointWrap);
            

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
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.S))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.W))
            {
                movementDirection += Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.A))
            {
                movementDirection += Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.D))
            {
                movementDirection -= Vector2.UnitX;
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


    }
    
}
