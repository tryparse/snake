using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.Settings
{
    public interface IGameSettings
    {
        bool IsLoggingEnabled { get; set; }

        bool IsDebugRenderingEnabled { get; set; }

        int ScreenWidth { get; set; }

        int ScreenHeight { get; set; }

        int TileWidth { get; set; }

        int TileHeight { get; set; }

        int MapWidth { get; set; }

        int MapHeight { get; set; }

        void ReadFromApplicationSettings(NameValueCollection appSettings);

        int DefaultSnakeMovingTime { get; set; }

        int CurrentSnakeMovingTime { get; set; }

        int DefaultMoveIntervalTime { get; set; }

        int CurrentMoveIntervalTime { get; set; }
    }
}
