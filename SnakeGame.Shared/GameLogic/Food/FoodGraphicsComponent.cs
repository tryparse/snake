using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.Graphics;
using System;
using MonoGame.Extended.Sprites;

namespace SnakeGame.Shared.GameLogic.Food
{
    public class FoodGraphicsComponent : IGraphics2DComponent
    {
        private readonly IFoodEntity _food;
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly Sprite _sprite;

        public FoodGraphicsComponent(IFoodEntity food, IGraphicsSystem graphicsSystem)
        {
            _food = food ?? throw new ArgumentNullException(nameof(food));
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));

            var textureRegion = _graphicsSystem.TextureManager.TextureRegions["Fruit"];

            _sprite = new Sprite(textureRegion)
            {
                Position = _food.Position,
                Color = Color.White,
                Rotation = _food.Rotation,
                Scale = new Vector2(_food.Size.Width / (float)textureRegion.Bounds.Width,
                    _food.Size.Height / (float)textureRegion.Bounds.Height),
                Origin = new Vector2(textureRegion.Width / 2f, textureRegion.Height / 2f),
                Effect = SpriteEffects.None
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateSprite();

            if (_graphicsSystem.GraphicsSettings.IsRenderingEnabled)
            {
                _sprite.Draw(spriteBatch);
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            UpdateSprite();

            if (_graphicsSystem.GraphicsSettings.IsDebugRenderingEnabled)
            {
                spriteBatch.DrawRectangle(_food.Bounds, Color.Orange, 2);
                spriteBatch.DrawString(_graphicsSystem.DebugSpriteFont, _food.Position.ToString(), _food.Position,
                    Color.Black);
            }
        }

        private void UpdateSprite()
        {
            _sprite.Position = _food.Position;
            _sprite.Rotation = _food.Rotation;
        }
    }
}
