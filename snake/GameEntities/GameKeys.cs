using Microsoft.Xna.Framework.Input;

namespace snake.GameEntities
{
    public class GameKeys
    {
        public GameKeys(Keys switchPause, Keys switchDebugRendering, Keys switchRendering, Keys exit, Keys debugInfo)
        {
            SwitchPause = switchPause;
            SwitchDebugRendering = switchDebugRendering;
            SwitchRendering = switchRendering;
            Exit = exit;
            DebugInfo = debugInfo;
        }

        public Keys SwitchPause { get; }

        public Keys SwitchDebugRendering { get; }

        public Keys SwitchRendering { get; }

        public Keys Exit { get; }

        public Keys DebugInfo { get; }
    }
}
