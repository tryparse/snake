using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Renderers;
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
        private readonly IRenderingSystem _renderingSystem;

        public SnakeGraphicsComponent(ISnake snake, IRenderingSystem renderingSystem)
        {
            _snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _renderingSystem = renderingSystem ?? throw new ArgumentNullException(nameof(renderingSystem));
        }

        public void Draw(GameTime gameTime)
        {
            if (_renderingSystem.RenderSettings.IsRenderingEnabled)
            {
                Rendering();
            }

            if (_renderingSystem.RenderSettings.IsDebugRenderingEnabled)
            {
                DebugRendering();
            }
        }

        private void Rendering()
        {
            foreach (var segment in _snake.Segments)
            {
                var selectedTexture = SelectTexture(segment);

                var rotation = DirectionHelper.GetRotation(segment.Direction);
                var origin = new Vector2(selectedTexture.Bounds.Width / 2f, selectedTexture.Bounds.Height / 2f);

                var scale = CalculateScale(segment, selectedTexture);

                _renderingSystem.SpriteBatch.Draw(
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
        }

        private TextureRegion2D SelectTexture(ISnakeSegment segment)
        {
            TextureRegion2D selectedTexture;

            if (segment.Equals(_snake.Segments.First()))
            {
                selectedTexture = _renderingSystem.TextureManager.TextureRegions["Head"];
            }
            else if (segment.Equals(_snake.Segments.Last()))
            {
                selectedTexture = _renderingSystem.TextureManager.TextureRegions["Tail"];
            }
            else
            {
                selectedTexture = _renderingSystem.TextureManager.TextureRegions["Part"];
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

            return scale * .9f;
        }

        private void DebugRendering()
        {
            foreach (var segment in _snake.Segments)
            {
                var rotation = DirectionHelper.GetRotation(segment.Direction);

                _renderingSystem.SpriteBatch.DrawRectangle(segment.Bounds, Color.Red);
                _renderingSystem.SpriteBatch.DrawLine(segment.Bounds.Center.ToVector2(), segment.Bounds.Width / 2f, rotation, Color.Red);
                _renderingSystem.SpriteBatch.DrawString(_renderingSystem.DebugSpriteFont, segment.Position.ToString(), segment.Position, Color.Red);
            }
        }
    }
}
