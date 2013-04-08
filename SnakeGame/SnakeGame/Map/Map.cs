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
    class Map
    {
        int mapIndex;

        Rectangle window;

        ContentManager content;

        Texture2D backgroundMap;

        public Map(ContentManager content, int mapIndex, Rectangle window)
        {
            this.content = content;
            this.mapIndex = mapIndex;
            this.window = window;

            this.updateRegion();
        }

        private void updateRegion()
        {
        }

        public void setBackgroundMap(String imageName)
        {
            this.backgroundMap = content.Load<Texture2D>(@"Images\Map\" + imageName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundMap, new Rectangle(0, 0, window.Width, window.Height), Color.White);
        }
    }
}
