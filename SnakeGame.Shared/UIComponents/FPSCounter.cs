using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.UIComponents
{
    class FpsCounter : DrawableGameComponent
    {
        private readonly Vector2 _position;
        private readonly Color _color;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;
        private readonly FramesPerSecondCounter _fps;
        private readonly IGraphicsSystem _graphicsSystem;

        public FpsCounter(Game game, Vector2 position, SpriteBatch spriteBatch, SpriteFont font, Color color, IGraphicsSystem graphicsSystem) : base(game)
        {
            this._position = position;
            this._color = color;
            this._spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            this._font = font;
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));

            this._fps = new FramesPerSecondCounter();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _fps.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsSystem.SpriteBatch.Begin(samplerState: SamplerState.PointClamp,
                sortMode: SpriteSortMode.BackToFront, blendState: BlendState.AlphaBlend,
                transformMatrix: _graphicsSystem.Camera2D.GetViewMatrix());

            base.Draw(gameTime);

            _fps.Draw(gameTime);

            _spriteBatch.DrawString(_font, _fps.FramesPerSecond.ToString(), _position, _color);

            _graphicsSystem.SpriteBatch.End();
        }
    }
}
