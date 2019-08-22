using Microsoft.Xna.Framework.Input;
using SnakeGame.Shared.Settings.Interfaces;

namespace SnakeGame.Shared.Settings.Implementation
{
    public class GameKeys : IGameKeys
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