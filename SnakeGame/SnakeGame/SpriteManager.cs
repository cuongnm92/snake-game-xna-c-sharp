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
        SpriteBatch spriteBatch;
        Rectangle window;

        GameState gameState = GameState.MainMenu;

        //Font
        SpriteFont font;

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

        //Menu Object
        Menu mainMenu;
        Menu selectDiff;

        //Input Object
        Cursor cursor;

        Vector2 center;
        Vector2 mousePosition;

        bool useMouse;

        public SpriteManager(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //Loading fonts
            font = Game.Content.Load<SpriteFont>(@"Fonts\Arial");

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

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            window = Game.Window.ClientBounds;

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

                        break;
                    }

                case GameState.SelectDifficulty:
                    {
                        selectDiff.Update(gameTime);
                        if (selectDiff.ClickedButtonPurpose > 0) updateDifficult(gameTime, selectDiff.ClickedButtonPurpose);
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
            }

            if (gameState != GameState.InGame)
            {
                cursor.Draw(gameTime, spriteBatch, mousePosition);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
