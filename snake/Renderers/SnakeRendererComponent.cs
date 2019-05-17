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
using snake.Interfaces;
using snake.Logging;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.Logging;

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

        private readonly TextureRegion2D _textureHead;
        private readonly TextureRegion2D _texturePart;
        private readonly TextureRegion2D _textureTail;
        
        public SnakeRendererComponent(SpriteBatch spriteBatch, SpriteFont spriteFont, IRenderSettings settings, ILogger logger, SnakeComponent snake, TextureAtlas textureRegions, int drawOrder = 0)
        {
            this._spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            this._spriteFont = spriteFont ?? throw new ArgumentNullException(nameof(spriteFont));
            this.RenderSettings = settings;
            this._logger = logger;
            this._snake = snake ?? throw new ArgumentNullException(nameof(snake));
            this._textureRegions = textureRegions ?? throw new ArgumentNullException(nameof(textureRegions));
            this._textureHead = _textureRegions.GetRegion("Head");
            this._texturePart = _textureRegions.GetRegion("Part");
            this._textureTail = _textureRegions.GetRegion("Tail");
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
            for (int i = 0; i < _snake.Parts.Count; i++)
            {
                var part = _snake.Parts[i];

                TextureRegion2D selectedTexture = SelectTexture(i);

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

        private TextureRegion2D SelectTexture(int partIndex)
        {
            TextureRegion2D selectedTexture;

            if (partIndex == HEAD_INDEX)
            {
                selectedTexture = _textureHead;
            }
            else if (partIndex == _snake.Parts.Count - 1)
            {
                // TODO: Add more variations
                selectedTexture = _textureTail;
            }
            else
            {
                selectedTexture = _texturePart;
            }

            return selectedTexture;
        }

        private void DebugRendering()
        {
            for (int i = 0; i < _snake.Parts.Count; i++)
            {
                var part = _snake.Parts[i];

                var rotation = DirectionHelper.GetRotation(part.Direction);

                _spriteBatch.DrawRectangle(part.Bounds, Color.Red);
                _spriteBatch.DrawLine(part.Bounds.Center.ToVector2(), part.Bounds.Width / 2f, rotation, Color.Red);
            }
        }
    }
}
