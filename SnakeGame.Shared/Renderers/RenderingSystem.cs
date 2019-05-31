using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.Common.ResourceManagers;

namespace SnakeGame.Shared.Renderers
{
    public class RenderingSystem : IRenderingSystem
    {
        public RenderingSystem(IRenderSettings renderSettings, SpriteBatch spriteBatch, SpriteFont spriteFont, SpriteFont debugSpriteFont, ITextureManager textureManager)
        {
            RenderSettings = renderSettings;
            SpriteBatch = spriteBatch;
            SpriteFont = spriteFont;
            DebugSpriteFont = debugSpriteFont;
            TextureManager = textureManager;
        }

        public IRenderSettings RenderSettings { get; }

        public SpriteBatch SpriteBatch { get; }

        public SpriteFont DebugSpriteFont { get; }

        public ITextureManager TextureManager { get; }

        public SpriteFont SpriteFont { get; }
    }
}
