using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace SnakeGame
{
    class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int addPoint = 1;

        ContentManager content;

        SpriteBatch spriteBatch;
        Rectangle window;

        GameState gameState = GameState.MainMenu;

        //Font
        SpriteFont font;
        SpriteFont scoreFont;

        //Texture
        Texture2D mouseTexture;

        Texture2D mainMenuBackGround;
        Texture2D nameMenuTexture;
        Texture2D startGameButton;
        Texture2D loadGameButton;
        Texture2D achGameButton;
        Texture2D optionGameButton;
        Texture2D aboutGameButton;
        Texture2D exitGameButton;

        Texture2D selectDiffBackGround;
        Texture2D easyGameButton;
        Texture2D normalGameButton;
        Texture2D hardGameButton;

        Texture2D ratTexture;
        Texture2D snakeTexture;
        Texture2D playerTexture;

        // Scoring
        int evadingSpritePointValue = 0;

        //Menu Object
        Menu mainMenu;
        Menu selectDiff;

        //Input Object
        Cursor cursor;

        Vector2 center;
        Vector2 mousePosition;

        bool useMouse;

        // Map
        int mapIndex;
        Map map;

        //Sprite
        Player player;
        List<Rat> rats = new List<Rat>();

        // Variables for spawning new rats
        int ratSpawnMinMilliseconds = 1000;
        int ratSpawnMaxMilliseconds = 2000;
        int ratMinSpeed = 2;
        int ratMaxSpeed = 6;
        int nextSpawnTime = 0;

        //Spawn time variables
        int nextSpawnTimeChange = 5000;
        int timeSinceLastSpawnTimeChange = 0;

        // Powerup stuff
        int powerUpExpiration = 0;

        public SpriteManager(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //Loading fonts
            font = Game.Content.Load<SpriteFont>(@"Fonts\Arial");
            scoreFont = Game.Content.Load<SpriteFont>(@"Fonts\Score");

            //Loading texture for input
            mouseTexture = Game.Content.Load<Texture2D>(@"Images\Input\cursor");

            //Loading texture for menu
            mainMenuBackGround = Game.Content.Load<Texture2D>(@"Images\Abstract-Snake");
            nameMenuTexture = Game.Content.Load<Texture2D>(@"Images\logo");
            startGameButton = Game.Content.Load<Texture2D>(@"Images\Button\StartGameMenuButton");
            loadGameButton = Game.Content.Load<Texture2D>(@"Images\Button\LoadGameMenuButton");
            achGameButton = Game.Content.Load<Texture2D>(@"Images\Button\ScoreMainMenuButton");
            optionGameButton = Game.Content.Load<Texture2D>(@"Images\Button\OptionGameMenuButton");
            aboutGameButton = Game.Content.Load<Texture2D>(@"Images\Button\AboutGameMenuButton");
            exitGameButton = Game.Content.Load<Texture2D>(@"Images\Button\ExitGameMenuButton");

            selectDiffBackGround = Game.Content.Load<Texture2D>(@"Images\diffbackground");
            easyGameButton = Game.Content.Load<Texture2D>(@"Images\Button\EasyMenuButton");
            normalGameButton = Game.Content.Load<Texture2D>(@"Images\Button\NormalMenuButton");
            hardGameButton = Game.Content.Load<Texture2D>(@"Images\Button\HardMenuButton");

            ratTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\rat");
            snakeTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\snakee");
            playerTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\snakee");

            Point frameSize = new Point(startGameButton.Height, startGameButton.Width);

            //Cursor Object
            this.mousePosition = new Vector2(300, 300);
            cursor = new Cursor(mouseTexture);

            //Menu Object
            int w = window.Width / 4;
            int h = window.Height / 4;

            center = new Vector2(w, h);

            mainMenu = new Menu(frameSize, font, window);
            mainMenu.AddButton(startGameButton, center + new Vector2(300, 250), 1);
            mainMenu.AddButton(loadGameButton, center + new Vector2(300, 300), 2);
            mainMenu.AddButton(achGameButton, center + new Vector2(300, 350), 3);
            mainMenu.AddButton(optionGameButton, center + new Vector2(300, 400), 4);
            mainMenu.AddButton(aboutGameButton, center + new Vector2(300, 450), 5);
            mainMenu.AddButton(exitGameButton, center + new Vector2(300, 500), 6);

            // Select Difficult 
            selectDiff = new Menu(frameSize, font, window);
            selectDiff.AddButton(easyGameButton, center + new Vector2(240, 100), 1);
            selectDiff.AddButton(normalGameButton, center + new Vector2(320, 190), 2);
            selectDiff.AddButton(hardGameButton, center + new Vector2(400, 280), 3);

            //selectDiff.AddButton();

            //Sprite
            player = new Player(playerTexture, new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2), new Point(96, 96), 10, new Point(0, 0), new Vector2(6, 6), 60);


            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void setContentManager(ContentManager content)
        {
            this.content = content;
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            window = Game.Window.ClientBounds;

            // Initialize spawn time
            ResetSpawnTime();

            this.mapIndex = 1;
            map = new Map(content, mapIndex, window);
            map.setBackgroundMap("map-background-1");

            base.Initialize();
        }

        public void updateDifficult(GameTime gameTime, int purpose)
        {
            gameState = GameState.InGame;
            if (purpose == 1)
            {

            }
            else if (purpose == 2)
            {
            }
            else
            {
            }
        }

        private void UpdateSpawnSprite(GameTime gameTime)
        {
            nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0)
            {
                SpawnRat();

                // Reset spawn timer
                ResetSpawnTime();
            }

            // Adjust sprite spawn times
            AdjustSpawnTimes(gameTime);

            // Expire Powerups?
            CheckPowerUpExpiration(gameTime);
        }

        private void UpdateSpriteMovement(GameTime gameTime, Rectangle window)
        {
            foreach (Rat r in rats)
                r.Update(gameTime, window);
        }

        private void CheckCollision(GameTime gameTime)
        {
            for (int i = 0; i < rats.Count; i++)
            {
                Rat r = rats[i];

                // Check for collisions
                if (r.collisionRect.Intersects(player.collisionRect))
                {
                    //Updata score value

                    player.UpdateScore(addPoint);

                    // Remove collided sprite from the game
                    rats.RemoveAt(i);
                    --i;
                }

                // Remove object if it is out of bounds
                if (r.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    rats.RemoveAt(i);
                    --i;
                }
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (useMouse)
            {
                MouseState mouseState = Mouse.GetState();
                mousePosition.X = mouseState.X;
                mousePosition.Y = mouseState.Y;
            }

            switch (gameState)
            {
                case GameState.MainMenu:
                    {
                        mainMenu.Update(gameTime);
                        if (mainMenu.ClickedButtonPurpose == 1)
                            gameState = GameState.SelectDifficulty;
                        else if (mainMenu.ClickedButtonPurpose == 2)
                            gameState = GameState.LoadGame;
                        else if (mainMenu.ClickedButtonPurpose == 3)
                            gameState = GameState.Score;
                        else if (mainMenu.ClickedButtonPurpose == 4)
                            gameState = GameState.Option;
                        else if (mainMenu.ClickedButtonPurpose == 5)
                            gameState = GameState.About;
                        else if (mainMenu.ClickedButtonPurpose == 6)
                            gameState = GameState.Exit;
                        break;
                    }

                case GameState.SelectDifficulty:
                    {
                        selectDiff.Update(gameTime);
                        if (selectDiff.ClickedButtonPurpose > 0) updateDifficult(gameTime, selectDiff.ClickedButtonPurpose);
                        break;
                    }
                case GameState.InGame:
                    {
                        player.Update(gameTime, window);
                        UpdateSpawnSprite(gameTime);
                        UpdateSpriteMovement(gameTime, window);
                        CheckCollision(gameTime);
                        break;
                    }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();


            switch (gameState)
            {
                case GameState.MainMenu:
                    {
                        useMouse = true;
                        spriteBatch.Draw(mainMenuBackGround, new Rectangle(0, 0, window.Width, window.Height), Color.White);

                        spriteBatch.Draw(
                            nameMenuTexture, center + new Vector2(320, 0),
                            new Rectangle(0, 0, nameMenuTexture.Width, nameMenuTexture.Height), Color.White, 0,
                            new Vector2(nameMenuTexture.Width / 2f, nameMenuTexture.Height / 2f), 1f, SpriteEffects.None, 0);

                        mainMenu.Draw(gameTime, spriteBatch);
                        break;
                    }
                case GameState.SelectDifficulty:
                    {
                        useMouse = true;
                        spriteBatch.Draw(selectDiffBackGround, new Rectangle(0, 0, window.Width, window.Height), Color.White);
                        selectDiff.Draw(gameTime, spriteBatch);
                        break;
                    }
                case GameState.InGame:
                    {
                        GraphicsDevice.Clear(Color.Green);
                        map.Draw(spriteBatch);
                        player.Draw(gameTime, spriteBatch);

                        // Draw rats
                        foreach (Rat r in rats)
                            r.Draw(gameTime, spriteBatch);

                        //Draw score 
                        // Draw fonts
                        spriteBatch.DrawString(scoreFont,
                            "Score: " + player.getScoreValue(),
                            new Vector2(10, 10), Color.BlueViolet,
                            0, Vector2.Zero,
                            1, SpriteEffects.None, 1);

                        break;
                    }
            }

            if (gameState != GameState.InGame)
            {
                cursor.Draw(gameTime, spriteBatch, mousePosition);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }

        public bool StopGame()
        {
            return (gameState == GameState.Exit);
        }

        // Return current position of the player sprite
        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }

        private void ResetSpawnTime()
        {
            // Set the next spawn time for an enemy
            nextSpawnTime = ((SnakeGame)Game).rnd.Next(
                ratSpawnMinMilliseconds,
                ratSpawnMaxMilliseconds);
        }

        private void SpawnRat()
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;

            // Default frame size
            Point frameSize = new Point(40, 40);
            Point currentFrame = new Point(0, 0);

            // Randomly choose which side of the screen to place enemy,
            // then randomly create a position along that side of the screen
            // and randomly choose a speed for the enemy
            switch (((SnakeGame)Game).rnd.Next(4))
            {
                case 0: // LEFT to RIGHT
                    position = new Vector2(
                        -frameSize.X, ((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));

                    speed = new Vector2(((SnakeGame)Game).rnd.Next(
                        ratMinSpeed,
                        ratMaxSpeed), 0);

                    currentFrame.Y = 2;
                    break;

                case 1: // RIGHT to LEFT
                    position = new
                        Vector2(
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                        ((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));

                    speed = new Vector2(-((SnakeGame)Game).rnd.Next(
                       ratMinSpeed, ratMaxSpeed), 0);

                    currentFrame.Y = 1;
                    break;

                case 2: // BOTTOM to TOP
                    position = new Vector2(((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X),
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

                    speed = new Vector2(0,
                       -((SnakeGame)Game).rnd.Next(ratMinSpeed,
                       ratMaxSpeed));

                    currentFrame.Y = 3;
                    break;

                case 3: // TOP to BOTTOM
                    position = new Vector2(((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X), -frameSize.Y);

                    speed = new Vector2(0,
                        ((SnakeGame)Game).rnd.Next(ratMinSpeed,
                        ratMaxSpeed));
                    break;
            }

            // Create the sprite
            rats.Add(
                new Rat(ratTexture, position, new Point(40, 40), 10, currentFrame, speed, null, this, 1f, 150, evadingSpritePointValue));
        }

        protected void AdjustSpawnTimes(GameTime gameTime)
        {
            // If the spawn max time is > 500 milliseconds
            // decrease the spawn time if it is time to do
            // so based on the spawn-timer variables
            if (ratSpawnMaxMilliseconds > 500)
            {
                timeSinceLastSpawnTimeChange += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastSpawnTimeChange > nextSpawnTimeChange)
                {
                    timeSinceLastSpawnTimeChange -= nextSpawnTimeChange;
                    if (ratSpawnMaxMilliseconds > 1000)
                    {
                        ratSpawnMaxMilliseconds -= 100;
                        ratSpawnMinMilliseconds -= 100;
                    }
                    else
                    {
                        ratSpawnMaxMilliseconds -= 10;
                        ratSpawnMinMilliseconds -= 10;
                    }
                }
            }
        }

        protected void CheckPowerUpExpiration(GameTime gameTime)
        {
            // Is a power-up active?
            if (powerUpExpiration > 0)
            {
                // Decrement power-up timer
                powerUpExpiration -= gameTime.ElapsedGameTime.Milliseconds;
                if (powerUpExpiration <= 0)
                {
                    // If power-up timer has expired, end all power-ups
                    powerUpExpiration = 0;
                    player.ResetScale();
                    player.ResetSpeed();
                }
            }
        }
    }
}
