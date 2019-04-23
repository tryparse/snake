using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using snake.Common;

namespace snake.Renderers
{
    public class SnakeRenderer : IRenderer2D
    {
        private readonly Snake _snake;
        private readonly TextureAtlas _textureRegions;
        private readonly SpriteBatch _spriteBatch;

        private Dictionary<SnakeDirection, float> _rotations;
        private TextureRegion2D _texture;

        public SnakeRenderer(Snake snake, TextureAtlas textureRegions, SpriteBatch spriteBatch)
        {
            this._snake = snake ?? throw new ArgumentNullException(nameof(snake));
            this._textureRegions = textureRegions ?? throw new ArgumentNullException(nameof(textureRegions));
            this._spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));

            _texture = _textureRegions.GetRegion("Head");

            _rotations = new Dictionary<SnakeDirection, float>
            {
                { SnakeDirection.Up, MathHelper.ToRadians(-90) },
                { SnakeDirection.Right, MathHelper.ToRadians(0) },
                { SnakeDirection.Down, MathHelper.ToRadians(90) },
                { SnakeDirection.Left, MathHelper.ToRadians(180) }
            };
        }

        public void Draw(GameTime gameTime)
        {
            
            var destinationRectangle = new RectangleF(
                Vector2.Multiply(_snake.Position, 0f).ToPoint(),
                new Point(TileMetrics.Width, TileMetrics.Height));
            var origin = Vector2.Zero;

            _spriteBatch.Draw(
                texture:_texture.Texture,
                destinationRectangle: destinationRectangle.ToRectangle(),
                sourceRectangle: _texture.Bounds,
                color: Color.White,
                rotation: _rotations[_snake.CurrentDirection],
                origin: origin,
                effects: SpriteEffects.None,
                layerDepth: .1f
            );
        }
    }
}
