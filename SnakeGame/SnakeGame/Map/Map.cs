using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame
{
    class Map
    {
        protected const int maxn = 1000;

        protected int currentMapIndex;
        protected int lastMapIndex;

        protected Point mapDimension;
        protected Point titleSize;


        protected Texture2D mapTex;

        protected String mapFileName;

        protected int[][] mapTitleValue;

        public Map(Texture2D mapTex, Point mapDimension, Point titleSize)
        {
            this.currentMapIndex = 1;
            this.lastMapIndex = 1;
            this.mapFileName = "";

            this.mapDimension = mapDimension;
            this.mapTex = mapTex;
            this.titleSize = titleSize;

            this.mapTitleValue = new int[maxn][];
            for (int i = 0; i < maxn; i++)
            {
                this.mapTitleValue[i] = new int[maxn];
            }
        }

        private void ReadingMapFile(String mapFileName)
        {
            StreamReader reader = new StreamReader(TitleContainer.OpenStream(mapFileName));

            for (int i = 0; i < mapDimension.X; i++)
                for (int j = 0; j < mapDimension.Y; j++)
                {

                }
        }
    }
}
