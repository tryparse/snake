using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.Graphics;
using System;

namespace SnakeGame.Shared.GameLogic.Food
{
    public class FoodGraphicsComponent : IGraphics2DComponent
    {
        private readonly IFoodEntity _food;
        private readonly IGraphicsSystem _graphicsSystem;

        public FoodGraphicsComponent(IFoodEntity food, IGraphicsSystem graphicsSystem)
        {
            _food = food ?? throw new ArgumentNullException(nameof(food));
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (_graphicsSystem.GraphicsSettings.IsRenderingEnabled)
            {
                var t = _graphicsSystem.TextureManager.TextureRegions["Fruit"];

                var origin = new Vector2(t.Width / 2f, t.Height / 2f);
                var scale = new Vector2(_food.Size.Width / (float)t.Bounds.Width, _food.Size.Height / (float)t.Bounds.Height);

                spriteBatch.Draw(
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

            if (_graphicsSystem.GraphicsSettings.IsDebugRenderingEnabled)
            {
                DebugDraw(spriteBatch);
            }
        }

        private void DebugDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(_food.Bounds, Color.Orange, 1);
            spriteBatch.DrawString(_graphicsSystem.DebugSpriteFont, _food.Position.ToString(), _food.Position, Color.Black);
        }
    }
}
