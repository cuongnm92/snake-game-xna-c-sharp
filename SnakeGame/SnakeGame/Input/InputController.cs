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
    class InputController
    {
        ContentManager content;
        Texture2D mouseTexture;

        Vector2 mousePosition;
        Cursor cursor;

        public InputController(ContentManager content)
        {
            this.content = content;
            this.initMouse();
        }

        private void initMouse()
        {
            //Loading texture for input
            this.mouseTexture = content.Load<Texture2D>(@"Images\Input\cursor");
            cursor = new Cursor(mouseTexture);
            this.mousePosition = new Vector2(300, 300);
        }

        public void drawCursor(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MouseState mouseState = Mouse.GetState();
            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;

            cursor.Draw(gameTime, spriteBatch, mousePosition);
        }
    }
}
