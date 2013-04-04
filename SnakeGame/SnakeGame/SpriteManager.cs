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

        //Menu Texture
        Texture2D mainMenuBackGround;
        Texture2D nameMenuTexture;
        Texture2D startGameButton;
        Texture2D loadGameButton;
        Texture2D achGameButton;
        Texture2D optionGameButton;
        Texture2D exitGameButton;

        //Menu Object
        Menu mainMenu;

        Vector2 center;

        public SpriteManager(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //Loading fonts
            font = Game.Content.Load<SpriteFont>(@"Fonts\Arial");


            //Loading image for menu
            mainMenuBackGround = Game.Content.Load<Texture2D>(@"Images\Abstract-Snake");
            nameMenuTexture = Game.Content.Load<Texture2D>(@"Images\logo");
            startGameButton = Game.Content.Load<Texture2D>(@"Images\Button\StartGameMenuButton");
            loadGameButton = Game.Content.Load<Texture2D>(@"Images\Button\LoadGameMenuButton");
            achGameButton = Game.Content.Load<Texture2D>(@"Images\Button\AchievementGameMenuButton");
            optionGameButton = Game.Content.Load<Texture2D>(@"Images\Button\OptionGameMenuButton");
            exitGameButton = Game.Content.Load<Texture2D>(@"Images\Button\ExitGameMenuButton");


            Point frameSize = new Point(startGameButton.Height, startGameButton.Width);

            //Menu Object
            int w = window.Width / 4;
            int h = window.Height / 4;

            center = new Vector2(w, h);

            mainMenu = new Menu(frameSize, font, window);
            mainMenu.AddButton(startGameButton, center + new Vector2(300, 250));
            mainMenu.AddButton(loadGameButton, center + new Vector2(300, 300));
            mainMenu.AddButton(achGameButton, center + new Vector2(300, 350));
            mainMenu.AddButton(optionGameButton, center + new Vector2(300, 400));
            mainMenu.AddButton(exitGameButton, center + new Vector2(300, 450));


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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
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
                        spriteBatch.Draw(mainMenuBackGround, new Rectangle(0, 0, window.Width, window.Height), Color.White);

                        spriteBatch.Draw(
                            nameMenuTexture, center + new Vector2(320, 0),
                            new Rectangle(0, 0, nameMenuTexture.Width, nameMenuTexture.Height), Color.White, 0,
                            new Vector2(nameMenuTexture.Width / 2f, nameMenuTexture.Height / 2f), 1f, SpriteEffects.None, 0);

                        mainMenu.Draw(gameTime, spriteBatch);
                        break;
                    }
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
