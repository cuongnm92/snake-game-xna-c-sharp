using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SnakeGame
{
    abstract class Sprite
    {
        //Direction of sprite : DOWN LEFT RIGHT UP
        protected int[] dx = new int[4] { 0, -1, 1, 0 };
        protected int[] dy = new int[4] { 1, 0, 0, -1 };

        protected bool isAlive;

        protected Texture2D texImage;

        protected Point frameSize;
        protected Point currentFrame;

        protected Vector2 position;
        protected Vector2 speed;

        protected int direction;
        protected int gWidth;
        protected int gHeight;

        public Sprite(Texture2D texImage, Point frameSize, Vector2 position, Vector2 speed, int gWidth, int gHeight)
        {
            this.texImage = texImage;
            this.frameSize = frameSize;
            this.position = position;
            this.speed = speed;
            this.gWidth = gWidth;
            this.gHeight = gHeight;

            this.currentFrame = new Point(0, 0);
            this.isAlive = true;
            this.direction = 0;
        }

        public virtual bool outOfBound(Vector2 spritePosition)
        {
            int xMax = gWidth - texImage.Width;
            int yMax = gHeight - texImage.Height;
            int xMin = 0;
            int yMin = 0;

            if (spritePosition.X > xMax || spritePosition.X < xMin || spritePosition.Y > yMax || spritePosition.Y < yMin)
            {
                return true;
            }

            return false;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw the sprite
            spriteBatch.Draw(texImage,
                position,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero,
                1f, SpriteEffects.None, 0);
        }
    }
}
