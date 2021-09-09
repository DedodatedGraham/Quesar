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
        
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public int uiStage;
        public UiManager _uiManager;


        private Texture2D background;
        private Texture2D altbackground;
        public Texture2D buttonv1;

        

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
            publicFont = Content.Load<SpriteFont>("Font");

            _uiManager = new UiManager(GraphicsDevice, buttonv1, _graphics);


            // Inform Myra that external text input is available
            // So it stops translating Keys to chars



        }

        protected override void Update(GameTime gameTime)
        {
            if (uiStage == 3)
                Exit();

            // TODO: Add your update logic here

            //This updates Camera movement to the testship
            //camMovementSpeed = 2;
            //zooom = 0.2f;
            //testShip.camera.Move(GetMovementDirection() * camMovementSpeed );
            //testShip.camera.ZoomIn(GetZoom() * zooom);



           uiStage = _uiManager.UpdateManager(gameTime, uiStage);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();


            //This is the background and always the last thing on the screen, 
            _spriteBatch.Draw(getUiStageBackground(), new Vector2(0, 0), Color.White);
            _uiManager.Draw(_spriteBatch, publicFont, uiStage);
            //Heres the ui menu drawing


            _spriteBatch.End();
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


            }
            return background;



        }


    }
    
}
