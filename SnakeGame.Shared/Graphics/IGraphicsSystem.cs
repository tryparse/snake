using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Shared.Common.ResourceManagers;

namespace SnakeGame.Shared.Graphics
{
    public interface IGraphicsSystem
    {
        IGraphicsSettings GraphicsSettings { get; }

        SpriteFont DebugSpriteFont { get; }

        SpriteFont SpriteFont { get; }

        ITextureManager TextureManager { get;}

        Effect LoadEffect(string path);

        OrthographicCamera Camera2D { get; }

        GraphicsDevice GraphicsDevice { get; }
    }
}
