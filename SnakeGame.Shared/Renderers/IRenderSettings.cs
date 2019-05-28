namespace SnakeGame.Shared.Renderers
{
    public interface IRenderSettings
    {
        bool IsRenderingEnabled { get; set; }

        bool IsDebugRenderingEnabled { get; set; }
    }
}
