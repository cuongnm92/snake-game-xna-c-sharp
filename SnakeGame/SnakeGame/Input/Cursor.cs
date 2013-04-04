using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame
{
    class Cursor
    {
        protected Texture2D mouseTex;

        public Cursor(Texture2D mouseTex)
        {
            this.mouseTex = mouseTex;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(mouseTex, position, null, Color.White, 0f,
                    Vector2.Zero, 0.2f, SpriteEffects.None, 0f);
        }
    }
}
