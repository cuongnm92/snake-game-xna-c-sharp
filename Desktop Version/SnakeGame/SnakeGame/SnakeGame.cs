using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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

    enum GameMode { Space, Classic };
    enum GameState { MainMenu, Score, Option, About, Exit, SelectControl, SelectMode, InGame, Won, Lost, Results, GameOver };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SnakeGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState gameState = GameState.MainMenu;

        SpaceModeManager spaceMode;

        // XACT stuff
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue trackCue;

        InputController input;
        MainMenu mainMenu;
        ModeSelecting modeMenu;

        public int score;
        SpriteFont scoreFont;

        ScoreManager scoreManager;

        // Random number generator
        public Random rnd { get; private set; }

        Rectangle window;

        public SnakeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            rnd = new Random();
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
            graphics.PreferredBackBufferWidth = GetScreenDimensions.X;
            graphics.PreferredBackBufferHeight = GetScreenDimensions.Y;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();


            window = Window.ClientBounds;
            input = new InputController(Content);
            mainMenu = new MainMenu(Content, window);
            modeMenu = new ModeSelecting(Content, window);

            spaceMode = new SpaceModeManager(this);
            Components.Add(spaceMode);
            spaceMode.Enabled = false;
            spaceMode.Visible = false;

            scoreManager = new ScoreManager();

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

            // Load the XACT data
            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            scoreFont = Content.Load<SpriteFont>(@"Fonts\Score");

            // Start the soundtrack audio
            trackCue = soundBank.GetCue("track");
            trackCue.Play();

            // Play the start sound
            soundBank.PlayCue("start");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || gameState == GameState.Exit)
                this.Exit();

            switch (gameState)
            {
                case GameState.MainMenu:
                    {
                        mainMenu.Update(gameTime);
                        gameState = mainMenu.getCurrentState(gameTime);
                        break;
                    }
                case GameState.SelectMode:
                    {
                        modeMenu.Update(gameTime);
                        spaceMode.Enabled = true;
                        spaceMode.Visible = true;
                        gameState = GameState.InGame;
                        // gameState = GameState.GameOver;
                        break;
                    }
                case GameState.Score:
                    {
                        break;
                    }
                case GameState.InGame:
                    {
                        if (spaceMode.isGameEnd())
                        {
                            this.score = spaceMode.getCurrentScore();
                            spaceMode.Enabled = false;
                            spaceMode.Visible = false;
                            gameState = GameState.GameOver;
                        }
                        break;
                    }
                case GameState.GameOver:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            gameState = GameState.MainMenu;
                        }

                        break;
                    }
            }


            // Update the audio engine
            audioEngine.Update();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (gameState)
            {
                case GameState.MainMenu:
                    {
                        spriteBatch.Begin();

                        mainMenu.Draw(gameTime, spriteBatch);
                        input.drawCursor(gameTime, spriteBatch);

                        spriteBatch.End();

                        break;
                    }
                case GameState.SelectMode:
                    {
                        spriteBatch.Begin();

                        modeMenu.Draw(gameTime, spriteBatch);
                        input.drawCursor(gameTime, spriteBatch);

                        spriteBatch.End();

                        break;
                    }
                case GameState.Score:
                    {
                        spriteBatch.Begin();

                        spriteBatch.Draw(Content.Load<Texture2D>(@"Images\top-score"), new Rectangle(0, 0, window.Width, window.Height), Color.White);

                        scoreManager.readFile();

                        string text;
                        for (int i = 0; i < scoreManager.getLimit(); i++)
                        {
                            text = i + "/ " + scoreManager.getScore(i).ToString();
                            // Find the center of the string
                            Vector2 FontOrigin = scoreFont.MeasureString(text) / 2;
                            // Draw the string
                            spriteBatch.DrawString(scoreFont, text, new Vector2(400, 275 + 100 * i), Color.LightGreen,
                                0, FontOrigin, 3.0f, SpriteEffects.None, 0.5f);

                        }

                        spriteBatch.End();

                        break;
                    }
                case GameState.InGame:
                    {
                        break;
                    }
                case GameState.GameOver:
                    {
                        spriteBatch.Begin();

                        spriteBatch.Draw(Content.Load<Texture2D>(@"Images\game_over"), new Rectangle(0, 0, window.Width, window.Height), Color.White);

                        string gameover = "Your score is : " + this.score;

                        spriteBatch.DrawString(scoreFont, gameover,
                                               new Vector2(680, 470),
                                               Color.White, 0, scoreFont.MeasureString(gameover) / 2, 1.5f, SpriteEffects.None, 0.5f);


                        gameover = "press Enter key to return main menu";

                        spriteBatch.DrawString(scoreFont, gameover, new Vector2(678, 530), Color.LightGreen,
                                0, scoreFont.MeasureString(gameover) / 2, 2.0f, SpriteEffects.None, 0.5f);

                        spriteBatch.End();
                        break;
                    }
            }

            base.Draw(gameTime);
        }

        public void PlayCue(string cueName)
        {
            soundBank.PlayCue(cueName);
        }

        public Point GetScreenDimensions
        {
            get
            {
                return new Point(graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);
            }
        }
    }
}
