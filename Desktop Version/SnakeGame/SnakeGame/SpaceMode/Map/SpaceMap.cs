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
    class SpaceMap
    {
        const int maxChangeTime = 16000;
        int changeTime = 0;

        int mapIndex;

        Rectangle window;

        ContentManager content;

        List<Texture2D> backgroundMap = new List<Texture2D>();
        
        public SpaceMap(ContentManager content, int mapIndex, Rectangle window)
        {
            this.content = content;
            this.mapIndex = mapIndex;
            this.window = window;

            this.updateRegion();
            this.updateBackgroundMap();
        }

        private void updateRegion()
        {
        }

        public void updateBackgroundMap()
        {
            this.backgroundMap.Add(content.Load<Texture2D>(@"Images\Map\space-1"));
            this.backgroundMap.Add(content.Load<Texture2D>(@"Images\Map\space-2"));
            this.backgroundMap.Add(content.Load<Texture2D>(@"Images\Map\space-3"));
            this.backgroundMap.Add(content.Load<Texture2D>(@"Images\Map\space-4"));
            this.backgroundMap.Add(content.Load<Texture2D>(@"Images\Map\space-5"));
        }

        public void Update(GameTime gameTime)
        {
            changeTime++;
            if (changeTime >= maxChangeTime)
            {
                this.mapIndex++;
                changeTime = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            mapIndex = mapIndex % this.backgroundMap.Count();
            spriteBatch.Draw(backgroundMap[mapIndex], new Rectangle(0, 0, window.Width, window.Height), Color.White);
        }
    }
}
