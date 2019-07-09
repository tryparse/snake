using SnakeGame.Shared.Graphics;

namespace snake.Renderers
{
    public class RenderSettings : IGraphicsSettings
    {
        public bool IsRenderingEnabled { get; set; }

        public bool IsDebugRenderingEnabled { get; set; }
    }
}
