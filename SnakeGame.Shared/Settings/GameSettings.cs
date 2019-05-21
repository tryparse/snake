using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public int MapWidth { get; set; }

        public int MapHeight { get; set; }

        public void ReadFromApplicationSettings(NameValueCollection appSettings)
        {
            bool.TryParse(appSettings["IsLoggingEnabled"], out var isLoggingEnabled);

            IsLoggingEnabled = isLoggingEnabled;

            bool.TryParse(appSettings["IsDebugRenderingEnabled"], out var isDebugRenderingEnabled);

            IsDebugRenderingEnabled = isDebugRenderingEnabled;

            int.TryParse(appSettings["ScreenWidth"], out var screenWidth);

            ScreenWidth = screenWidth;

            int.TryParse(appSettings["ScreenHeight"], out var screenHeight);

            ScreenHeight = screenHeight;

            int.TryParse(appSettings["TileWidth"], out var tileWidth);

            TileWidth = tileWidth;

            int.TryParse(appSettings["TileHeight"], out var tileHeight);

            TileHeight = tileHeight;

            int.TryParse(appSettings["MapWidth"], out var mapWidth);

            MapWidth = mapWidth;

            int.TryParse(appSettings["MapHeight"], out var mapHeight);

            MapHeight = mapHeight;
        }
    }
}
