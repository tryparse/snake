using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using snake.Common;
using snake.GameEntities.Fruit;
using snake.Interfaces;

namespace snake.Renderers
{
    class FruitRendererComponent : IRenderer2D
    {
        private readonly Fruit _fruit;
        private readonly SpriteBatch _spriteBatch;
        private readonly TextureRegion2D _textureRegion;

        private int _drawOrder;
        private bool _visible;
        private float _rotation;

        public FruitRendererComponent(Fruit fruit, SpriteBatch spriteBatch, TextureRegion2D textureRegion, IRenderSettings settings)
        {
            this._fruit = fruit;
            this._spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            this._textureRegion = textureRegion ?? throw new ArgumentNullException(nameof(textureRegion));
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));

            Initialize();
        }

        public IRenderSettings Settings { get; }

        public int DrawOrder => _drawOrder;

        public bool Visible => _visible;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public void Initialize()
        {
            _drawOrder = 0;
            _visible = true;
            _rotation = 0f;
        }

        public void Draw(GameTime gameTime)
        {
            if (Settings.IsRenderingEnabled)
            {
                Render();
            }

            if (Settings.IsDebugRenderingEnabled)
            {
                DebugRender();
            }
        }

        private void DebugRender()
        {

        }

        private void Render()
        {
            var origin = new Vector2(_textureRegion.Width / 2, _textureRegion.Height / 2);
            var scale = new Vector2(_fruit.Size.X / _textureRegion.Bounds.Width, _fruit.Size.Y / _textureRegion.Bounds.Height);

            _spriteBatch.Draw(
                    texture: _textureRegion.Texture,
                    position: _fruit.Position,
                    sourceRectangle: _textureRegion.Bounds,
                    color: Color.White,
                    rotation: _rotation,
                    scale: scale,
                    origin: origin,
                    effects: SpriteEffects.None,
                    layerDepth: BackToFrontLayers.Fruit
                );
        }
    }
}
