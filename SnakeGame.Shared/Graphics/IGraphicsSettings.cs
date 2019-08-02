namespace SnakeGame.Shared.Graphics
{
    public interface IGraphicsSettings
    {
        bool IsRenderingEnabled { get; set; }

        bool IsDebugRenderingEnabled { get; set; }

        void ToggleRenderingEnabled();

        void ToggleDebugRenderingEnabled();
    }
}
