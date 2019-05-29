using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.Common.ResourceManagers;

namespace SnakeGame.Shared.Renderers
{
    public class RenderingSystem : IRenderingSystem
    {
        public RenderingSystem(IRenderSettings renderSettings, SpriteBatch spriteBatch, SpriteFont spriteFont, ITextureManager textureManager)
        {
            RenderSettings = renderSettings;
            SpriteBatch = spriteBatch;
            SpriteFont = spriteFont;
            TextureManager = textureManager;
        }

        public IRenderSettings RenderSettings { get; }

        public SpriteBatch SpriteBatch { get; }

        public SpriteFont SpriteFont { get; }

        public ITextureManager TextureManager { get; }
    }
}
