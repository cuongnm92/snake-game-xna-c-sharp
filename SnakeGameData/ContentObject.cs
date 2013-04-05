using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;

namespace SnakeGameData
{
    public abstract class ContentObject
    {
        /// <summary>
        /// The name of the content pipeline asset that contained this object.
        /// </summary>
        private string assetName;

        /// <summary>
        /// The name of the content pipeline asset that contained this object.
        /// </summary>
        [ContentSerializerIgnore]
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
    }
}
