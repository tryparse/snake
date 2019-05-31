using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.Common.ResourceManagers;

namespace SnakeGame.Shared.Renderers
{
    public interface IRenderingSystem
    {
        IRenderSettings RenderSettings { get; }

        SpriteBatch SpriteBatch { get; }

        SpriteFont DebugSpriteFont { get; }

        SpriteFont SpriteFont { get; }

        ITextureManager TextureManager { get;}
    }
}
