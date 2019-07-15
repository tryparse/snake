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

        bool IsRenderingEnabled { get; set; }

        int ScreenWidth { get; set; }

        int ScreenHeight { get; set; }

        int TileSize { get; set; }

        int MapWidth { get; set; }

        int MapHeight { get; set; }

        void ReadFromApplicationSettings(NameValueCollection appSettings);

        int DefaultMoveIntervalTime { get; set; }

        int CurrentMoveIntervalTime { get; set; }
    }
}
