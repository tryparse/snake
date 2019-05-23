using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using snake.GameEntities;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Renderers;
using System;

namespace snake.Renderers
{
    public class SnakeRendererComponent : IRenderer2D
    {
        private const int HEAD_INDEX = 0;
        private readonly ILogger _logger;
        private readonly SnakeComponent _snake;
        private readonly TextureAtlas _textureRegions;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;
        private int _drawOrder;
        private bool _isVisible;

        public SnakeRendererComponent(SpriteBatch spriteBatch, SpriteFont spriteFont, IRenderSettings settings, ILogger logger, SnakeComponent snake, TextureAtlas textureRegions, int drawOrder = 0)
        {
            this._spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            this._spriteFont = spriteFont ?? throw new ArgumentNullException(nameof(spriteFont));
            this.RenderSettings = settings;
            this._logger = logger;
            this._snake = snake ?? throw new ArgumentNullException(nameof(snake));
            this._textureRegions = textureRegions ?? throw new ArgumentNullException(nameof(textureRegions));
            this._drawOrder = drawOrder;

            Initialize();
        }

        public IRenderSettings RenderSettings { get; }

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
            this._isVisible = true;
        }

        public void Draw(GameTime gameTime)
        {
            if (RenderSettings.IsRenderingEnabled)
            {
                Rendering();
            }

            if (RenderSettings.IsDebugRenderingEnabled)
            {
                DebugRendering();
            }
        }

        private void Rendering()
        {
            foreach (var part in _snake.Parts)
            {
                TextureRegion2D selectedTexture = SelectTexture(part);

                var rotation = DirectionHelper.GetRotation(part.Direction);
                var origin = new Vector2(selectedTexture.Bounds.Width / 2f, selectedTexture.Bounds.Height / 2f);

                Vector2 scale = CalculateScale(part, selectedTexture);

                _spriteBatch.Draw(
                    texture: selectedTexture.Texture,
                    position: part.Position,
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

        private Vector2 CalculateScale(SnakePart part, TextureRegion2D selectedTexture)
        {
            Vector2 scale;
            // Hack of scaling, for cases when TileWidth != TileHeight
            if (part.Direction == Direction.Right
                || part.Direction == Direction.Left)
            {
                scale = new Vector2(part.Size.Width / selectedTexture.Bounds.Width, part.Size.Height / selectedTexture.Bounds.Height);
            }
            else
            {
                scale = new Vector2(part.Size.Height / selectedTexture.Bounds.Height, part.Size.Width / selectedTexture.Bounds.Width);
            }

            return scale;
        }

        private TextureRegion2D SelectTexture(SnakePart part)
        {
            TextureRegion2D selectedTexture;

            if (part == _snake.Parts.First.Value)
            {
                selectedTexture = _textureRegions["Head"];
            }
            else if (part == _snake.Parts.Last.Value)
            {
                // TODO: Add more variations
                selectedTexture = _textureRegions["Tail"];
            }
            else
            {
                selectedTexture = _textureRegions["Part"];
            }

            return selectedTexture;
        }

        private void DebugRendering()
        {
            foreach (var part in _snake.Parts)
            {
                var rotation = DirectionHelper.GetRotation(part.Direction);

                _spriteBatch.DrawRectangle(part.Bounds, Color.Red);
                _spriteBatch.DrawLine(part.Bounds.Center.ToVector2(), part.Bounds.Width / 2f, rotation, Color.Red);
            }
        }
    }
}
