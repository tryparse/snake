using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.Common
{
    public static class TileMetrics
    {
        [Obsolete("Use IGameSettings.TileWidth property instead")]
        public const int Width = 50;

        [Obsolete("Use IGameSettings.TileHeight property instead")]
        public const int Height = 50;

        [Obsolete("Use IGameSettings.TileWidth and IGameSettings.TileHeight")]
        public static Vector2 Size => new Vector2(Width, Height);
    }
}
