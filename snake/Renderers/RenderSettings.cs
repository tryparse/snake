using SnakeGame.Shared.Renderers;

namespace snake.Renderers
{
    public class RenderSettings : IRenderSettings
    {
        public bool IsRenderingEnabled { get; set; }

        public bool IsDebugRenderingEnabled { get; set; }
    }
}
