using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Shared.Common.ResourceManagers;

namespace SnakeGame.Shared.Graphics
{
    public class GraphicsSystem : IGraphicsSystem
    {
        private readonly ContentManager _contentManager;

        public GraphicsSystem(IGraphicsSettings graphicsSettings, ContentManager contentManager, SpriteFont spriteFont, SpriteFont debugSpriteFont, ITextureManager textureManager, Camera2D camera2D)
        {
            GraphicsSettings = graphicsSettings;
            _contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
            SpriteFont = spriteFont;
            DebugSpriteFont = debugSpriteFont;
            TextureManager = textureManager;
            Camera2D = camera2D;
        }

        public IGraphicsSettings GraphicsSettings { get; }

        public SpriteFont DebugSpriteFont { get; }

        public ITextureManager TextureManager { get; }

        public SpriteFont SpriteFont { get; }

        public Camera2D Camera2D { get; }

        public Effect LoadEffect(string path)
        {
            return _contentManager.Load<Effect>(path);
        }
    }
}
