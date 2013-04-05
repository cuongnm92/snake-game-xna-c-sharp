using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SnakeGame
{
    abstract class Animal
    {
        protected bool isAlive;

        protected Texture2D imageTex;

        protected Point sheetSize;
        protected Point frameSize;
        protected Point currentFrame = new Point(0, 0);
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame = 20;
        protected const int defualtMillisecondsPerFrame = 16;

        protected Vector2 position = Vector2.Zero;
        protected Vector2 speed = Vector2.Zero;
        protected float rotation = 0;
        protected Rectangle window;

        public Animal(Texture2D imageTex, Point sheetSize, Point frameSize, Vector2 position, Vector2 speed, int millisecondsPerFrame, Rectangle window)
        {
            this.imageTex = imageTex;

            this.sheetSize = sheetSize;
            this.frameSize = frameSize;

            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;

            this.window = window;
        }

        public virtual void Update(GameTime gameTime)
        {
            // Get the next frame in the sprite sheet
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;

                ++currentFrame.X;
                if (currentFrame.X > sheetSize.X - 1)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;

                    if (currentFrame.Y > sheetSize.Y - 1)
                        currentFrame.Y = 0;
                }
            }
        }

        public virtual bool IsAlive
        {
            get
            {
                return (!outOfBounds(window, 30) && isAlive);
            }
        }

        public bool outOfBounds(Rectangle rect, float offset)
        {
            return (position.X < 0 - offset || position.X > rect.Width + offset
                || position.Y < 0 - offset || position.Y > rect.Height + offset);
        }
    }
}
