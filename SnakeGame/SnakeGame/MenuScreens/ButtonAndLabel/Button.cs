using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame
{
    class Button
    {
        protected Texture2D buttonTex;
        protected Point currentFrame;
        protected Point frameSize;

        protected Vector2 position;
        protected Vector2 FontOrigin;

        public Button(Texture2D buttonTex, Vector2 position)
        {
            this.buttonTex = buttonTex;
            this.position = position;
            
            this.currentFrame = new Point(0, 0);
            this.frameSize = new Point(buttonTex.Width, buttonTex.Height);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                buttonTex,
                position,
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                Color.White, 0, new Vector2(frameSize.X / 2f, frameSize.Y / 2f), 1f, SpriteEffects.None, 0);

        }
    }
}
