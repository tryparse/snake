using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common.ResourceManagers;
using SnakeGame.Shared.Renderers;

namespace SnakeGame.Shared.Graphics
{
    public interface IGraphics2DComponent
    {
        IRenderSettings RenderSettings { get; }

        ITextureManager TextureManager { get; }

        void Draw(GameTime gameTime);
    }
}
