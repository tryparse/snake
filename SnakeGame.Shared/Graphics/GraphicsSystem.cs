using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.Common.ResourceManagers;

namespace SnakeGame.Shared.Graphics
{
    public class GraphicsSystem : IGraphicsSystem
    {
        public GraphicsSystem(IGraphicsSettings renderSettings, SpriteBatch spriteBatch, SpriteFont spriteFont, SpriteFont debugSpriteFont, ITextureManager textureManager)
        {
            RenderSettings = renderSettings;
            SpriteBatch = spriteBatch;
            SpriteFont = spriteFont;
            DebugSpriteFont = debugSpriteFont;
            TextureManager = textureManager;
        }

        public IGraphicsSettings RenderSettings { get; }

        public SpriteBatch SpriteBatch { get; }

        public SpriteFont DebugSpriteFont { get; }

        public ITextureManager TextureManager { get; }

        public SpriteFont SpriteFont { get; }
    }
}
