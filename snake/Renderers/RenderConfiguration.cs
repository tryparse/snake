using snake.Interfaces;

namespace snake.Renderers
{
    public class RenderConfiguration : IRenderSettings
    {
        public bool IsRenderingEnabled { get; set; }

        public bool IsDebugRenderingEnabled { get; set; }
    }
}
