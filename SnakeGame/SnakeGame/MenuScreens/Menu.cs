using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame
{
    class Menu
    {

        SpriteFont font;
        Rectangle window;

        Point frameSize;

        List<Button> buttons = new List<Button>();

        public Menu(Point frameSize, SpriteFont font, Rectangle window)
        {
            this.font = font;
            this.window = window;
            this.frameSize = frameSize;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Button button in buttons)
                button.Update(gameTime);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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
