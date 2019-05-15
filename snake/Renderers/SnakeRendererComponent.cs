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
        private const int HEAD_INDEX = 0;
        private readonly RenderConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly SnakeComponent _snake;
        private readonly TextureAtlas _textureRegions;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;
        private int _drawOrder;
        private bool _isVisible;

        private readonly TextureRegion2D _textureHead;
        private readonly TextureRegion2D _texturePart;
        
        public SnakeRendererComponent(SpriteBatch spriteBatch, SpriteFont spriteFont, RenderConfiguration configuration, ILogger logger, SnakeComponent snake, TextureAtlas textureRegions, int drawOrder = 0)
        {
            this._spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            this._spriteFont = spriteFont ?? throw new ArgumentNullException(nameof(spriteFont));
            this._configuration = configuration;
            this._logger = logger;
            this._snake = snake ?? throw new ArgumentNullException(nameof(snake));
            this._textureRegions = textureRegions ?? throw new ArgumentNullException(nameof(textureRegions));
            this._textureHead = _textureRegions.GetRegion("Head");
            this._texturePart = _textureRegions.GetRegion("Part");
            this._drawOrder = drawOrder;

            Initialize();
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
            this._isVisible = true;
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
            for (int i = 0; i < _snake.Parts.Count; i++)
            {
                var part = _snake.Parts[i];

                TextureRegion2D selectedTexture;

                if (i == HEAD_INDEX)
                {
                    selectedTexture = _textureHead;
                }
                else
                {
                    selectedTexture = _texturePart;
                    // TODO: Add more variations
                }

                var rotation = DirectionHelper.GetRotation(part.Direction);
                var origin = new Vector2(selectedTexture.Bounds.Width / 2f, selectedTexture.Bounds.Height / 2f);

                Vector2 scale;

                // Hack of scaling, don't know what to do :(
                if (part.Direction == Direction.Right
                    || part.Direction == Direction.Left)
                {
                    scale = new Vector2(part.Size.X / selectedTexture.Bounds.Width, part.Size.Y / selectedTexture.Bounds.Height);
                }
                else
                {
                    scale = new Vector2(part.Size.Y / selectedTexture.Bounds.Height, part.Size.X / selectedTexture.Bounds.Width);
                }

                

                _spriteBatch.Draw(
                    texture: selectedTexture.Texture,
                    position: part.Position,
                    sourceRectangle: selectedTexture.Bounds,
                    color: Color.White,
                    rotation: rotation,
                    scale: scale,
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

                var rotation = DirectionHelper.GetRotation(part.Direction);

                _spriteBatch.DrawRectangle(part.Bounds, Color.Red);
                _spriteBatch.DrawLine(part.Bounds.Center.ToVector2(), part.Bounds.Width / 2f, rotation, Color.Red);
            }
        }
    }
}
