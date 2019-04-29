using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using NLog;
using snake.Common;
using snake.GameEntities;

namespace snake.Renderers
{
    public class SnakeRenderer : IRenderer2D
    {
        private readonly ILogger _logger;
        private readonly Snake _snake;
        private readonly TextureAtlas _textureRegions;
        private readonly SpriteBatch _spriteBatch;

        private Dictionary<SnakeDirection, float> _rotations;
        private TextureRegion2D _textureHead;

        public SnakeRenderer(ILogger logger, Snake snake, TextureAtlas textureRegions, SpriteBatch spriteBatch)
        {
            this._logger = logger;
            this._snake = snake ?? throw new ArgumentNullException(nameof(snake));
            this._textureRegions = textureRegions ?? throw new ArgumentNullException(nameof(textureRegions));
            this._spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));

            this._textureHead = _textureRegions.GetRegion("Head");

            this._rotations = new Dictionary<SnakeDirection, float>
            {
                { SnakeDirection.Up, MathHelper.ToRadians(-90) },
                { SnakeDirection.Right, MathHelper.ToRadians(0) },
                { SnakeDirection.Down, MathHelper.ToRadians(90) },
                { SnakeDirection.Left, MathHelper.ToRadians(180) }
            };
        }

        public void Draw(GameTime gameTime)
        {
            var destinationRectangle = _snake.Bounds;
            var origin = new Vector2(_textureHead.Bounds.Width / 2, _textureHead.Bounds.Height / 2);

            _logger.Debug($"SnakeRenderer.Draw()");
            _logger.Debug($"{nameof(destinationRectangle)}={destinationRectangle}");
            _logger.Debug($"{nameof(origin)}={origin}");

            _spriteBatch.Draw(
                texture: _textureHead.Texture,
                destinationRectangle: destinationRectangle,
                sourceRectangle: _textureHead.Bounds,
                color: Color.White,
                rotation: 0,
                origin: origin,
                effects: SpriteEffects.None,
                layerDepth: .1f
            );

            _spriteBatch.DrawRectangle(destinationRectangle, Color.Red);
        }
    }
}
