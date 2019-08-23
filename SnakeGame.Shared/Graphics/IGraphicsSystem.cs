using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.Common.ResourceManagers;

namespace SnakeGame.Shared.Graphics
{
    public interface IGraphicsSystem
    {
        IGraphicsSettings GraphicsSettings { get; }

        SpriteBatch SpriteBatch { get; }

        SpriteFont DebugSpriteFont { get; }

        SpriteFont SpriteFont { get; }

        ITextureManager TextureManager { get;}
    }
}
