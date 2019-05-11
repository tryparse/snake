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
        public const int Width = 50;
        public const int Height = 50;

        public static Vector2 Size => new Vector2(Width, Height);
    }
}
