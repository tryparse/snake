﻿using System.Collections.Specialized;

namespace SnakeGame.Shared.Settings.Interfaces
{
    public interface IGameSettings
    {
        bool IsLoggingEnabled { get; set; }

        bool IsDebugRenderingEnabled { get; set; }

        bool IsRenderingEnabled { get; set; }

        int ScreenWidth { get; set; }

        int ScreenHeight { get; set; }

        bool IsFullScreen { get; set; }

        int TileSize { get; set; }

        int MapWidth { get; set; }

        int MapHeight { get; set; }

        void ReadFromApplicationSettings(NameValueCollection appSettings);

        /// <summary>
        /// Default move interval time (in milliseconds)
        /// </summary>
        int DefaultMoveIntervalTime { get; set; }

        /// <summary>
        /// Current move interval time (in milliseconds)
        /// </summary>
        int CurrentMoveIntervalTime { get; set; }

        /// <summary>
        /// Minimal move interval time (in milliseconds)
        /// </summary>
        int LimitMoveIntervalTime { get; set; }

        int RemainingLives { get; set; }

        bool IsShowDebugInfo { get; set; }

        void ResetMoveIntervalTime();

        void ToggleDebugInfo();
    }
}
