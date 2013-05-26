using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame
{
    abstract class Menu
    {
        protected Rectangle window;


        List<Button> buttons = new List<Button>();

        public Menu(Rectangle window)
        {
            this.window = window;
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Button button in buttons)
                button.Update(gameTime);

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
                button.Draw(gameTime, spriteBatch);
        }

        public virtual int ClickedButtonPurpose
        {
            get
            {
                foreach (Button button in buttons)
                    if (button.Clicked) return button.getPurpose;

                return 0;
            }
        }

        public void AddButton(Texture2D buttonTexture, Vector2 position, int purpose)
        {
            buttons.Add(new Button(buttonTexture, position, purpose));
        }
    }
}
