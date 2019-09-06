using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Shared.Graphics;
using System;

namespace SnakeGame.Shared.UIComponents
{
    internal class FpsCounter : IUiComponent
    {
        private readonly Vector2 _position;
        private readonly Color _color;
        private readonly SpriteFont _font;
        private readonly FramesPerSecondCounter _fps;
        private readonly IGraphicsSystem _graphicsSystem;

        public FpsCounter(Vector2 position, SpriteFont font, Color color, IGraphicsSystem graphicsSystem)
        {
            this._position = position;
            this._color = color;
            this._font = font;
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));

            this._fps = new FramesPerSecondCounter();
        }

        public void Update(GameTime gameTime)
        {
            _fps.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _fps.Draw(gameTime);

            spriteBatch.DrawString(_font, _fps.FramesPerSecond.ToString(), _position, _color);
        }
    }
}
