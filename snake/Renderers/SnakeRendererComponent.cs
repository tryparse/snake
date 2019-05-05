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
using snake.GameEntities;
using snake.Logging;

namespace snake.Renderers
{
    public class SnakeRendererComponent : IRenderer2D
    {
        private readonly RenderConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly Snake _snake;
        private readonly TextureAtlas _textureRegions;
        private readonly SpriteBatch _spriteBatch;

        private int _drawOrder;
        private bool _isVisible;

        private Dictionary<SnakeDirection, float> _rotations;
        private TextureRegion2D _textureHead;

        public SnakeRendererComponent(RenderConfiguration configuration, ILogger logger, Snake snake, TextureAtlas textureRegions, SpriteBatch spriteBatch, int drawOrder)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._snake = snake ?? throw new ArgumentNullException(nameof(snake));
            this._textureRegions = textureRegions ?? throw new ArgumentNullException(nameof(textureRegions));
            this._spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            this._drawOrder = drawOrder;
            this._textureHead = _textureRegions.GetRegion("Head");
            this._isVisible = true;

            this._rotations = new Dictionary<SnakeDirection, float>
            {
                { SnakeDirection.Up, MathHelper.ToRadians(-90) },
                { SnakeDirection.Right, MathHelper.ToRadians(0) },
                { SnakeDirection.Down, MathHelper.ToRadians(90) },
                { SnakeDirection.Left, MathHelper.ToRadians(180) }
            };
        }

        public int DrawOrder
        {
            get => _drawOrder;
            set
            {
                DrawOrderChanged?.Invoke(this, new EventArgs());
                _drawOrder = value;
            }
        }

        public bool Visible
        {
            get => _isVisible;
            set
            {
                VisibleChanged?.Invoke(this, new EventArgs());
                _isVisible = value;
            }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public void Initialize()
        {
        }

        public void Draw(GameTime gameTime)
        {
            if (_configuration.IsRenderingEnabled)
            {
                var origin = new Vector2(_textureHead.Bounds.Width / 2f, _textureHead.Bounds.Height / 2f);
                var destinationRectangle = _snake.Bounds;
                destinationRectangle.Offset(destinationRectangle.Width / 2f, destinationRectangle.Height / 2f);

                _spriteBatch.Draw(
                    texture: _textureHead.Texture,
                    destinationRectangle: destinationRectangle,
                    sourceRectangle: _textureHead.Bounds,
                    color: Color.White,
                    rotation: _rotations[_snake.CurrentDirection],
                    origin: origin,
                    effects: SpriteEffects.None,
                    layerDepth: 0f
                );
            }

            if (_configuration.IsDebugRenderingEnabled)
            {
                DebugDraw(gameTime);
            }
        }

        private void DebugDraw(GameTime gameTime)
        {
            _spriteBatch.DrawRectangle(_snake.Bounds, Color.Red, 3);
            _spriteBatch.DrawLine(_snake.Bounds.Center.ToVector2(), _snake.Bounds.Width / 2f, _rotations[_snake.CurrentDirection], Color.Red, 3f);
        }
    }
}
