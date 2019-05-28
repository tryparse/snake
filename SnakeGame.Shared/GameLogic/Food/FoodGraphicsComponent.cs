using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.Common.ResourceManagers;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Renderers;
using System;

namespace SnakeGame.Shared.GameLogic.Food
{
    public class FoodGraphicsComponent : IGraphics2DComponent
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;
        private readonly IFood _food;

        public FoodGraphicsComponent(SpriteBatch spriteBatch, SpriteFont spriteFont, IFood food, IRenderSettings renderSettings, ITextureManager textureManager)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            _spriteFont = spriteFont ?? throw new ArgumentNullException(nameof(spriteFont));
            _food = food ?? throw new ArgumentNullException(nameof(food));
            RenderSettings = renderSettings ?? throw new ArgumentNullException(nameof(renderSettings));
            TextureManager = textureManager ?? throw new ArgumentNullException(nameof(textureManager));
        }

        public IRenderSettings RenderSettings { get; }

        public ITextureManager TextureManager { get; }

        public void Draw(GameTime gameTime)
        {
            if (RenderSettings.IsRenderingEnabled)
            {
                Rendering();
            }

            if (RenderSettings.IsDebugRenderingEnabled)
            {
                DebugRendering();
            }
        }

        private void DebugRendering()
        {
            _spriteBatch.DrawRectangle(_food.Bounds, Color.Orange, 1);
            _spriteBatch.DrawString(_spriteFont, _food.Position.ToString(), _food.Position, Color.Black);
        }

        private void Rendering()
        {
            var t = TextureManager.TextureRegions["Fruit"];

            var origin = new Vector2(t.Width / 2, t.Height / 2);
            var scale = new Vector2(_food.Size.Width / t.Bounds.Width, _food.Size.Height / t.Bounds.Height);

            _spriteBatch.Draw(
                    texture: t.Texture,
                    position: _food.Position,
                    sourceRectangle: t.Bounds,
                    color: Color.White,
                    rotation: _food.Rotation,
                    scale: scale,
                    origin: origin,
                    effects: SpriteEffects.None,
                    layerDepth: LayerDepths.Fruit
                );
        }
    }
}
