using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class SnakeGraphicsComponent : IGraphics2DComponent
    {
        private readonly ISnake _snake;
        private readonly IGraphicsSystem _graphicsSystem;

        public SnakeGraphicsComponent(ISnake snake, IGraphicsSystem graphicsSystem)
        {
            _snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
        }

        public void Draw(GameTime gameTime)
        {
            if (_graphicsSystem.RenderSettings.IsRenderingEnabled)
            {
                Rendering();
            }

            if (_graphicsSystem.RenderSettings.IsDebugRenderingEnabled)
            {
                DebugRendering();
            }
        }

        private void Rendering()
        {
            DrawSegment(_snake.Head);

            foreach (var segment in _snake.Tail)
            {
                DrawSegment(segment);
            }
        }

        private void DrawSegment(ISnakeSegment segment)
        {
            var selectedTexture = SelectTexture(segment);

            var rotation = DirectionHelper.GetRotation(segment.Direction);
            var origin = new Vector2(selectedTexture.Bounds.Width / 2f, selectedTexture.Bounds.Height / 2f);

            var scale = CalculateScale(segment, selectedTexture);

            _graphicsSystem.SpriteBatch.Draw(
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

        private void DebugRendering()
        {
            DebugDrawSegment(_snake.Head);

            foreach (var segment in _snake.Tail)
            {
                DebugDrawSegment(segment);
            }
        }

        private void DebugDrawSegment(ISnakeSegment segment)
        {
            var rotation = DirectionHelper.GetRotation(segment.Direction);

            _graphicsSystem.SpriteBatch.DrawRectangle(segment.Bounds, Color.Red, 2);
            _graphicsSystem.SpriteBatch.DrawLine(segment.Bounds.Center.ToVector2(), segment.Bounds.Width / 2f, rotation, Color.Red);
            _graphicsSystem.SpriteBatch.DrawString(_graphicsSystem.DebugSpriteFont, segment.Position.ToString(), segment.Position, Color.Red);
        }
    }
}
