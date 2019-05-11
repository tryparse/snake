using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.Configuration
{
    public class GameConfiguration
    {
        public GameConfiguration()
        {
        }

        public bool IsLoggingEnabled { get; set; }

        public bool IsDebugRenderingEnabled { get; set; }

        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }
    }
}
