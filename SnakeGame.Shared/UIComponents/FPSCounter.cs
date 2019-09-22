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

        public FpsCounter(Vector2 position, SpriteFont font, Color color)
        {
            _position = position;
            _color = color;
            _font = font;

            _fps = new FramesPerSecondCounter();
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
