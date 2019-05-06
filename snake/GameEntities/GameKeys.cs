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
        private readonly Keys _switchPause;
        private readonly Keys _switchDebugRendering;
        private readonly Keys _exit;

        public GameKeys(Keys switchPause, Keys switchDebugRendering, Keys exit)
        {
            this._switchPause = switchPause;
            this._switchDebugRendering = switchDebugRendering;
            this._exit = exit;
        }

        public Keys SwitchPause => _switchPause;

        public Keys SwitchDebugRendering => _switchDebugRendering;

        public Keys Exit => _exit;
    }
}
