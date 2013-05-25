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
    class ModeSelecting : Menu
    {
        ContentManager content;

        Texture2D background;
        Texture2D space;
        Texture2D classic;

        Vector2 center;

        public ModeSelecting(ContentManager content, Rectangle window)
            : base(window)
        {
            this.content = content;
            this.window = window;

            this.updateTexture();
            this.buildMenu();
        }

        private void updateTexture()
        {
            this.background = this.content.Load<Texture2D>(@"Images\diffbackground");
            this.space = this.content.Load<Texture2D>(@"Images\Button\protection_mode");
            this.classic = this.content.Load<Texture2D>(@"Images\Button\splash_mode");
        }

        private void buildMenu()
        {
            //Menu Object
            int w = window.Width / 4;
            int h = window.Height / 4;

            center = new Vector2(w, h);

            this.AddButton(space, center + new Vector2(220, 175), 1);
            this.AddButton(classic, center + new Vector2(300, 250), 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(background, new Rectangle(0, 0, window.Width, window.Height), Color.White);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
