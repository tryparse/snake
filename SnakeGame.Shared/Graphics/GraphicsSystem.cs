using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.Common.ResourceManagers;

namespace SnakeGame.Shared.Graphics
{
    public class GraphicsSystem : IGraphicsSystem
    {
        public GraphicsSystem(IGraphicsSettings graphicsSettings, SpriteBatch spriteBatch, SpriteFont spriteFont, SpriteFont debugSpriteFont, ITextureManager textureManager)
        {
            GraphicsSettings = graphicsSettings;
            SpriteBatch = spriteBatch;
            SpriteFont = spriteFont;
            DebugSpriteFont = debugSpriteFont;
            TextureManager = textureManager;
        }

        public IGraphicsSettings GraphicsSettings { get; }

        public SpriteBatch SpriteBatch { get; }

        public SpriteFont DebugSpriteFont { get; }

        public ITextureManager TextureManager { get; }

        public SpriteFont SpriteFont { get; }
    }
}
