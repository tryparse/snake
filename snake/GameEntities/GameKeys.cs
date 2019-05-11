using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameEntities
{
    public class GameKeys
    {
        public GameKeys(Keys switchPause, Keys switchDebugRendering, Keys switchRendering, Keys exit)
        {
            this.SwitchPause = switchPause;
            this.SwitchDebugRendering = switchDebugRendering;
            this.SwitchRendering = switchRendering;
            this.Exit = exit;
        }

        public Keys SwitchPause { get; }

        public Keys SwitchDebugRendering { get; }

        public Keys SwitchRendering { get; }

        public Keys Exit { get; }
    }
}
