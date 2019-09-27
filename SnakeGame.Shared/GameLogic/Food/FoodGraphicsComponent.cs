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
        private readonly Transform2 _transform;

        public FoodGraphicsComponent(IFoodEntity food, IGraphicsSystem graphicsSystem)
        {
            _food = food ?? throw new ArgumentNullException(nameof(food));
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));

            var textureRegion = _graphicsSystem.TextureManager.TextureRegions["Fruit"];

            _transform = new Transform2(
                position: _food.Position, 
                rotation: _food.Rotation,
                scale: new Vector2(_food.Size.Width / (float) textureRegion.Bounds.Width,
                    _food.Size.Height / (float) textureRegion.Bounds.Height));

            _sprite = new Sprite(textureRegion)
            {
                Color = Color.White,
                Origin = new Vector2(textureRegion.Width / 2f, textureRegion.Height / 2f),
                Effect = SpriteEffects.None
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateSprite();

            if (_graphicsSystem.GraphicsSettings.IsRenderingEnabled)
            {
                spriteBatch.Draw(_sprite, _transform);
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch, Vector2 pov, float radius)
        {
            UpdateSprite();

            if (_graphicsSystem.GraphicsSettings.IsDebugRenderingEnabled)
            {
                spriteBatch.DrawRectangle(_food.Bounds, Color.Orange, 2);
                spriteBatch.DrawString(_graphicsSystem.DebugSpriteFont, _food.Position.ToString(), _food.Position,
                    Color.Orange);
            }
        }

        private void UpdateSprite()
        {
            _transform.Position = _food.Position;
            _transform.Rotation = _food.Rotation;
        }
    }
}
