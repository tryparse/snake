using SnakeGame.Shared.Settings.Interfaces;
using System.Collections.Specialized;

namespace SnakeGame.Shared.Settings.Implementation
{
    public class GameSettings : IGameSettings
    {
        public bool IsLoggingEnabled { get; set; }

        public bool IsDebugRenderingEnabled { get; set; }

        public bool IsRenderingEnabled { get; set; }

        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }

        public bool IsFullScreen { get; set; }

        public int TileSize { get; set; }

        public int MapWidth { get; set; }

        public int MapHeight { get; set; }

        public int DefaultMoveIntervalTime { get; set; }

        public int CurrentMoveIntervalTime { get; set; }

        public int LimitMoveIntervalTime { get; set; }

        public int RemainingLives { get; set; }
        public void ReadFromApplicationSettings(NameValueCollection appSettings)
        {
            bool.TryParse(appSettings["IsLoggingEnabled"], out var isLoggingEnabled);

            IsLoggingEnabled = isLoggingEnabled;

            bool.TryParse(appSettings["IsDebugRenderingEnabled"], out var isDebugRenderingEnabled);

            IsDebugRenderingEnabled = isDebugRenderingEnabled;

            bool.TryParse(appSettings["IsRenderingEnabled"], out var isRenderingEnabled);

            IsRenderingEnabled = isRenderingEnabled;

            int.TryParse(appSettings["ScreenWidth"], out var screenWidth);

            ScreenWidth = screenWidth;

            int.TryParse(appSettings["ScreenHeight"], out var screenHeight);

            ScreenHeight = screenHeight;

            bool.TryParse(appSettings["IsFullScreen"], out var isFullScreen);

            IsFullScreen = isFullScreen;

            int.TryParse(appSettings["TileSize"], out var tileSize);

            TileSize = tileSize;

            int.TryParse(appSettings["MapWidth"], out var mapWidth);

            MapWidth = mapWidth;

            int.TryParse(appSettings["MapHeight"], out var mapHeight);

            MapHeight = mapHeight;

            int.TryParse(appSettings["DefaultMoveIntervalTime"], out var defaultMoveIntervalTime);

            DefaultMoveIntervalTime = defaultMoveIntervalTime;
            CurrentMoveIntervalTime = defaultMoveIntervalTime;

            int.TryParse(appSettings["LimitMoveIntervalTime"], out var limitMoveIntervalTime);

            LimitMoveIntervalTime = limitMoveIntervalTime;

            int.TryParse(appSettings["RemainingLives"], out var remainingLives);

            RemainingLives = remainingLives;
        }

        public void ResetMoveIntervalTime()
        {
            CurrentMoveIntervalTime = DefaultMoveIntervalTime;
        }
    }
}
