namespace SnakeGame.Shared.Graphics
{
    public class GraphicsSettings : IGraphicsSettings
    {
        public bool IsRenderingEnabled { get; set; }

        public bool IsDebugRenderingEnabled { get; set; }
        public void ToggleRenderingEnabled()
        {
            IsRenderingEnabled = !IsRenderingEnabled;
        }

        public void ToggleDebugRenderingEnabled()
        {
            IsDebugRenderingEnabled = !IsDebugRenderingEnabled;
        }
    }
}
