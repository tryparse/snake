using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.Common.ResourceManagers;

namespace SnakeGame.Shared.Graphics
{
    public class GraphicsSystem : IGraphicsSystem
    {
        private readonly ContentManager _contentManager;

        public GraphicsSystem(IGraphicsSettings graphicsSettings, ContentManager contentManager, SpriteBatch spriteBatch, SpriteFont spriteFont, SpriteFont debugSpriteFont, ITextureManager textureManager)
        {
            GraphicsSettings = graphicsSettings;
            _contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
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

        public Effect LoadEffect(string path)
        {
            return _contentManager.Load<Effect>(path);
        }
    }
}
