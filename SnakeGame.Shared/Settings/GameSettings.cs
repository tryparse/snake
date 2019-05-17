using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.Settings
{
    public class GameSettings : IGameSettings
    {
        public bool IsLoggingEnabled { get; set; }

        public bool IsDebugRenderingEnabled { get; set; }

        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }

        public int TileWidth { get; set; }

        public int TileHeight { get; set; }
    }
}
