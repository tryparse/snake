using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.Common.ResourceManagers;
using SnakeGame.Shared.GameLogic.Fruit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.Renderers
{
    public class FruitRendererComponent : IRenderer2D
    {
        private readonly Fruit _fruit;
        private readonly SpriteBatch _spriteBatch;
        private readonly ITextureManager _textureManager;
        

        private int _drawOrder;
        private bool _visible;
        private float _rotation;

        public FruitRendererComponent(Fruit fruit, SpriteBatch spriteBatch, ITextureManager textureManager, IRenderSettings settings)
        {
            _fruit = fruit ?? throw new ArgumentNullException(nameof(fruit));
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            _textureManager = textureManager ?? throw new ArgumentNullException(nameof(textureManager));
            RenderSettings = settings ?? throw new ArgumentNullException(nameof(settings));

            Initialize();
        }

        public IRenderSettings RenderSettings { get; }

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
            if (RenderSettings.IsRenderingEnabled)
            {
                Render();
            }

            if (RenderSettings.IsDebugRenderingEnabled)
            {
                DebugRender();
            }
        }

        private void DebugRender()
        {
            _spriteBatch.DrawRectangle(_fruit.Bounds, Color.Orange, 1);
        }

        private void Render()
        {
            var t = _textureManager.TextureRegions["Fruit"];

            var origin = new Vector2(t.Width / 2, t.Height / 2);
            var scale = new Vector2(_fruit.Size.Width / t.Bounds.Width, _fruit.Size.Height / t.Bounds.Height);

            _spriteBatch.Draw(
                    texture: t.Texture,
                    position: _fruit.Position,
                    sourceRectangle: t.Bounds,
                    color: Color.White,
                    rotation: _rotation,
                    scale: scale,
                    origin: origin,
                    effects: SpriteEffects.None,
                    layerDepth: LayerDepths.Fruit
                );
        }
    }
}
