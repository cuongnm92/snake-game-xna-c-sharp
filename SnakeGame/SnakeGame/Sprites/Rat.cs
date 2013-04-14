using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SnakeGame
{
    class Rat : Sprite
    {
        public Rat(Texture2D texImage, Point frameSize, Vector2 position, Vector2 speed, int gWidth, int gHeight)
            : base(texImage, frameSize, position, speed, gWidth, gHeight)
        {
        }

        public void Update(GameTime gameTime)
        {
            int changeDirection = -1;
            Vector2 newPosition = position + new Vector2(speed.X * dx[direction], speed.Y * dy[direction]);

            while (outOfBound(newPosition))
            {
                changeDirection = 1;
                direction = (direction + 1) % 4;
                newPosition = position + new Vector2(speed.X * dx[direction], speed.Y * dy[direction]);
            }

            position = newPosition;

            if (changeDirection != -1)
            {
                currentFrame = new Point(0, direction);
            }
            else
            {
                currentFrame = new Point((currentFrame.X + 1) % 4, currentFrame.Y);
            }
        }
    }
}
