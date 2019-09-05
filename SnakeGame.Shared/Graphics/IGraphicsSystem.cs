using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
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

        Effect LoadEffect(string path);

        Camera2D Camera2D { get; }
    }
}
