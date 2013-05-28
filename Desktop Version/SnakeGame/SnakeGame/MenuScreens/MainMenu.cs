using System;
using System.Collections.Generic;
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
    class MainMenu : Menu
    {
        ContentManager content;

        //Texture
        Texture2D mainMenuBackGround;
        Texture2D nameMenuTexture;
        Texture2D startGameButton;
        Texture2D achGameButton;
        Texture2D howPlayButton;
        Texture2D optionGameButton;
        Texture2D aboutGameButton;
        Texture2D exitGameButton;

        Vector2 center;

        public MainMenu(ContentManager content, Rectangle window)
            : base(window)
        {
            this.content = content;
            this.window = window;

            this.updateTexture();
            this.buildMenu();
        }

        private void updateTexture()
        {
            mainMenuBackGround = this.content.Load<Texture2D>(@"Images\Abstract-Snake");
            nameMenuTexture = this.content.Load<Texture2D>(@"Images\logo");
            startGameButton = this.content.Load<Texture2D>(@"Images\Button\StartGameMenuButton");
            achGameButton = this.content.Load<Texture2D>(@"Images\Button\ScoreMainMenuButton");
            howPlayButton = this.content.Load<Texture2D>(@"Images\Button\HowPlay");
            optionGameButton = this.content.Load<Texture2D>(@"Images\Button\OptionGameMenuButton");
            aboutGameButton = this.content.Load<Texture2D>(@"Images\Button\AboutGameMenuButton");
            exitGameButton = this.content.Load<Texture2D>(@"Images\Button\ExitGameMenuButton");
        }

        private void buildMenu()
        {
            //Menu Object
            int w = window.Width / 4;
            int h = window.Height / 4;

            center = new Vector2(w, h);

            this.AddButton(startGameButton, center + new Vector2(300, 250), 1);
            this.AddButton(achGameButton, center + new Vector2(300, 300), 2);
            this.AddButton(howPlayButton, center + new Vector2(300, 350), 3);
            //this.AddButton(aboutGameButton, center + new Vector2(300, 400), 4);
            this.AddButton(exitGameButton, center + new Vector2(300, 400), 5);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public GameState getCurrentState(GameTime gameTime)
        {
            if (this.ClickedButtonPurpose == 1) return GameState.InGame;
            if (this.ClickedButtonPurpose == 2) return GameState.Score;
            if (this.ClickedButtonPurpose == 3) return GameState.HowToPlay;
            if (this.ClickedButtonPurpose == 5) return GameState.Exit;

            return GameState.MainMenu;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(mainMenuBackGround, new Rectangle(0, 0, window.Width, window.Height), Color.White);

            spriteBatch.Draw(
                nameMenuTexture, center + new Vector2(320, 0),
                new Rectangle(0, 0, nameMenuTexture.Width, nameMenuTexture.Height), Color.White, 0,
                new Vector2(nameMenuTexture.Width / 2f, nameMenuTexture.Height / 2f), 1f, SpriteEffects.None, 0);

            base.Draw(gameTime, spriteBatch);
        }
    }
}
