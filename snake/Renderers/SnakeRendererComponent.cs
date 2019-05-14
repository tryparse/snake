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
using snake.GameEntities.Snake;
using snake.Logging;

namespace snake.Renderers
{
    public class SnakeRendererComponent : IRenderer2D
    {
        private readonly RenderConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly SnakeComponent _snake;
        private readonly TextureAtlas _textureRegions;
        private readonly SpriteBatch _spriteBatch;

        private int _drawOrder;
        private bool _isVisible;

        private readonly Dictionary<Direction, float> _rotations;
        private readonly TextureRegion2D _textureHead;
        private readonly TextureRegion2D _texturePart;
        
        public SnakeRendererComponent(SpriteBatch spriteBatch, RenderConfiguration configuration, ILogger logger, SnakeComponent snake, TextureAtlas textureRegions, int drawOrder = 0)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._snake = snake ?? throw new ArgumentNullException(nameof(snake));
            this._textureRegions = textureRegions ?? throw new ArgumentNullException(nameof(textureRegions));
            this._spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            this._drawOrder = drawOrder;
            this._textureHead = _textureRegions.GetRegion("Head");
            this._texturePart = _textureRegions.GetRegion("Part");
            this._isVisible = true;

            this._rotations = new Dictionary<Direction, float>
            {
                { Direction.Up, MathHelper.ToRadians(-90) },
                { Direction.Right, MathHelper.ToRadians(0) },
                { Direction.Down, MathHelper.ToRadians(90) },
                { Direction.Left, MathHelper.ToRadians(180) }
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
            // Nothing to initialize
        }

        public void Draw(GameTime gameTime)
        {
            if (_configuration.IsRenderingEnabled)
            {
                Rendering();
            }

            if (_configuration.IsDebugRenderingEnabled)
            {
                DebugRendering();
            }
        }

        private void Rendering()
        {
            var origin = new Vector2(_textureHead.Bounds.Width / 2f, _textureHead.Bounds.Height / 2f);

            for (int i = 0; i < _snake.Parts.Count; i++)
            {
                var part = _snake.Parts[i];

                TextureRegion2D selectedTexture;

                if (i == 0)
                {
                    selectedTexture = _textureHead;
                }
                else
                {
                    selectedTexture = _texturePart;
                    // TODO: Add more variations
                }

                var destinationRectangle = part.Bounds;
                destinationRectangle.Offset(destinationRectangle.Width / 2f, destinationRectangle.Height / 2f);

                _spriteBatch.Draw(
                    texture: selectedTexture.Texture,
                    destinationRectangle: destinationRectangle,
                    sourceRectangle: selectedTexture.Bounds,
                    color: Color.White,
                    rotation: _rotations[part.Direction],
                    origin: origin,
                    effects: SpriteEffects.None,
                    layerDepth: BackToFrontLayers.Snake
                );
            }
        }

        private void DebugRendering()
        {
            for (int i = 0; i < _snake.Parts.Count; i++)
            {
                var part = _snake.Parts[i];
                _spriteBatch.DrawRectangle(part.Bounds, Color.Red, 3);
                _spriteBatch.DrawLine(part.Bounds.Center.ToVector2(), part.Bounds.Width / 2f, _rotations[part.Direction], Color.Red, 3f);
            }
        }
    }
}
