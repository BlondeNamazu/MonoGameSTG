using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        SpriteFont font;
        Input input;
        Texture2D myship,bullet,enemy;
        List<Bullet> bulletList;
        Vector2 myshipPos;
        float timer;
        int bulletTimer;
        int enemyLife;
        float dp;
        private bool wasTouched;

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
#if ANDROID
            input = new Input_Android();
#elif WINDOWS
            input = new Input_Windows();
#endif
            base.Initialize();
            newGame();
        }

        private void newGame()
        {
            input.Init();
            myshipPos.X = ScreenSize.X / 2;
            myshipPos.Y = ScreenSize.Y * 4 / 5;
            timer = 0;
            bulletTimer = 10;
            bulletList = new List<Bullet>();
            enemyLife = 60;
            wasTouched = false;
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
            enemy = Content.Load<Texture2D>("enemy");
            bullet = Content.Load<Texture2D>("bullet");
            font = Content.Load<SpriteFont>("Font");
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
            wasTouched = input.isTouched;
            input.Update();
            base.Update(gameTime);
            if (enemyLife <= 0)
            {
                if (!wasTouched&&input.isTouched) newGame();
                return;
            }
            timer += 0.01f;
            bulletTimer--;
            if(bulletTimer <= 0 && input.isTouched)
            {
                Bullet bul = new Bullet(myshipPos+new Vector2(0,-bullet.Width/2));
                bulletList.Add(bul);
                bulletTimer = 10;
            }
            foreach (Bullet bul in bulletList) bul.Update();
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
            if (enemyLife > 0)
            {
                myshipPos.X = ScreenSize.X / 2 + input.vec.X;
                myshipPos.Y = ScreenSize.Y * 4 / 5 + input.vec.Y;
            }
            if (myshipPos.X < myship.Width * magnification / 2) myshipPos.X = (int)(myship.Width * magnification / 2);
            if (myshipPos.X > ScreenSize.X - myship.Width * magnification / 2) myshipPos.X = (int)(ScreenSize.X - myship.Width * magnification / 2);
            if (myshipPos.Y < myship.Height * magnification / 2) myshipPos.Y = (int)(myship.Height * magnification / 2);
            if (myshipPos.Y > ScreenSize.Y - myship.Height * magnification / 2) myshipPos.Y = (int)(ScreenSize.Y - myship.Height * magnification / 2);
            Rectangle rect = new Rectangle((int)myshipPos.X - (int)(myship.Width*magnification/2), (int)myshipPos.Y- (int)(myship.Height*magnification/2), (int)(myship.Width*magnification), (int)(myship.Height*magnification));

            Vector2 enemyPos = new Vector2((float)(ScreenSize.X / 2 + ScreenSize.X / 4 * Math.Sin(4 * timer)), (float)(ScreenSize.Y / 4 + ScreenSize.Y / 6 * Math.Sin(5 * timer)));
            Rectangle enemyRect = new Rectangle((int)enemyPos.X - (int)(enemy.Width * magnification / 2), (int)(enemyPos.Y) - (int)(enemy.Height * magnification / 2), (int)(enemy.Width * magnification), (int)(enemy.Height * magnification));

            spriteBatch.Begin();
            for(int i=bulletList.Count-1;i>=0;i--)
            {
                Bullet bul = bulletList[i];
                spriteBatch.Draw(bullet, new Rectangle((int)(bul.pos.X - bullet.Width*magnification), (int)(bul.pos.Y - bullet.Height*magnification), (int)(bullet.Width*magnification*2), (int)(bullet.Height*magnification*2)), Color.White);
                if (bul.hitEnemy(enemyPos, 48 * magnification))
                {
                    enemyLife--;
                    bulletList.Remove(bul);
                    continue;
                }
                if (bul.pos.Y < -bullet.Height) bulletList.Remove(bul);
            }
            spriteBatch.Draw(myship, rect, Color.White);
            if(enemyLife > 0)spriteBatch.Draw(enemy, enemyRect, Color.White);
            String output = enemyLife != 0 ? enemyLife.ToString() : "Game Clear!";
            Vector2 fontOrigin = font.MeasureString(output)/2;
            spriteBatch.DrawString(font, output, new Vector2(ScreenSize.X/2,ScreenSize.Y/5), Color.White,0,fontOrigin,2.0f*magnification,SpriteEffects.None,0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
