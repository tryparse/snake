using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Graphics;
using System;
using System.Linq;
using MonoGame.Extended.Sprites;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class SnakeGraphicsComponent : IGraphics2DComponent
    {
        private readonly ISnakeEntity _snake;
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly ISnakeMovementComponent _movement;
        private readonly IGameFieldEntity _gameField;

        private readonly Sprite _sprite;

        public SnakeGraphicsComponent(ISnakeEntity snake, IGraphicsSystem graphicsSystem, ISnakeMovementComponent movement, IGameFieldEntity gameField)
        {
            _snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            _movement = movement ?? throw new ArgumentNullException(nameof(movement));
            _gameField = gameField ?? throw new ArgumentNullException(nameof(gameField));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_graphicsSystem.GraphicsSettings.IsRenderingEnabled)
            {
                DrawSegment(spriteBatch, _snake.Head);

                foreach (var segment in _snake.Tail)
                {
                    if (_gameField.Bounds.Contains(segment.Position))
                    {
                        DrawSegment(spriteBatch, segment);
                    }
                }
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch, Vector2 pov, float radius)
        {
            if (_graphicsSystem.GraphicsSettings.IsDebugRenderingEnabled)
            {
                DebugDrawSegment(spriteBatch, _snake.Head);

                foreach (var segment in _snake.Tail)
                {
                    DebugDrawSegment(spriteBatch, segment);
                }

                spriteBatch.DrawCircle(pov, radius, 20, Color.Red);
            }
        }

        private void DrawSegment(SpriteBatch spriteBatch, ISnakeSegment segment)
        {
            var selectedTexture = SelectTexture(segment);

            var rotation = DirectionHelper.GetRotation(segment.Direction);
            var origin = new Vector2(selectedTexture.Bounds.Width / 2f, selectedTexture.Bounds.Height / 2f);

            var scale = CalculateScale(segment, selectedTexture);

            spriteBatch.Draw(
                texture: selectedTexture.Texture,
                position: segment.Position,
                sourceRectangle: selectedTexture.Bounds,
                color: Color.White,
                rotation: rotation,
                scale: scale,
                origin: origin,
                effects: SpriteEffects.None,
                layerDepth: LayerDepths.Snake
            );
        }

        private TextureRegion2D SelectTexture(ISnakeSegment segment)
        {
            TextureRegion2D selectedTexture;

            if (segment.Equals(_snake.Head))
            {
                selectedTexture = _graphicsSystem.TextureManager.TextureRegions["Head"];
            }
            else if (_snake.Tail.Any() && segment.Equals(_snake.Tail.Last()))
            {
                selectedTexture = _graphicsSystem.TextureManager.TextureRegions["Tail"];
            }
            else
            {
                selectedTexture = _graphicsSystem.TextureManager.TextureRegions["Part"];
            }

            return selectedTexture;
        }

        private Vector2 CalculateScale(ISnakeSegment segment, TextureRegion2D selectedTexture)
        {
            Vector2 scale;
            // Hack of scaling, for cases when TileWidth != TileHeight
            if (segment.Direction == Direction.Right
                || segment.Direction == Direction.Left)
            {
                scale = new Vector2(segment.Size.Width / selectedTexture.Bounds.Width, segment.Size.Height / selectedTexture.Bounds.Height);
            }
            else
            {
                scale = new Vector2(segment.Size.Height / selectedTexture.Bounds.Height, segment.Size.Width / selectedTexture.Bounds.Width);
            }

            return scale;
        }

        private void DebugDrawSegment(SpriteBatch spriteBatch, ISnakeSegment segment)
        {
            var rotation = DirectionHelper.GetRotation(segment.Direction);

            spriteBatch.DrawRectangle(segment.Bounds, Color.Red, 2);
            spriteBatch.DrawLine(segment.Bounds.Center.ToVector2(), segment.Bounds.Width / 2f, rotation, Color.Red);
            spriteBatch.DrawString(_graphicsSystem.DebugSpriteFont, segment.Position.ToString(), segment.Position, Color.Red);
        }
    }
}
