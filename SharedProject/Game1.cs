using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SharedProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    
    public class Game1 : Game
    {
        public static Game1 Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Input input;
        Texture2D myship;
        int x, y;
        float dp;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 360;
            graphics.PreferredBackBufferHeight = 640;
#if ANDROID
            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Instance = this;
            x = (int)ScreenSize.X / 2;
            y = (int)ScreenSize.Y * 4 / 5;
#if ANDROID
            input = new Input_Android();
#elif WINDOWS
            input = new Input_Windows();
#endif
            input.Init();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            myship = Content.Load<Texture2D>("myship");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here
            input.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            dp = ScreenSize.Y / 20;
            float magnification = dp / 64;
            x = (int)ScreenSize.X / 2 + (int)input.vec.X;
            y = (int)ScreenSize.Y * 4 / 5 + (int)input.vec.Y;
            if (x < myship.Width * magnification / 2) x = (int)(myship.Width * magnification / 2);
            if (x > ScreenSize.X - myship.Width * magnification / 2) x = (int)(ScreenSize.X - myship.Width * magnification / 2);
            if (y < myship.Height * magnification / 2) y = (int)(myship.Height * magnification / 2);
            if (y > ScreenSize.Y - myship.Height * magnification / 2) y = (int)(ScreenSize.Y - myship.Height * magnification / 2);
            Rectangle rect = new Rectangle(x- (int)(myship.Width*magnification/2), y- (int)(myship.Height*magnification/2), (int)(myship.Width*magnification), (int)(myship.Height*magnification));
            spriteBatch.Begin();
            spriteBatch.Draw(myship, rect, input.isTouched?Color.Red:Color.White);
            spriteBatch.End();
            //Console.WriteLine(rect.ToString());
            base.Draw(gameTime);
        }
    }
}
