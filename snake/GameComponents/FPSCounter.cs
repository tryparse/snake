using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameComponents
{
    class FPSCounter : DrawableGameComponent
    {
        private readonly Vector2 position;
        private readonly Color color;
        private readonly SpriteBatch spriteBatch;
        private readonly SpriteFont font;
        private readonly FramesPerSecondCounter fps;

        public FPSCounter(Game game, Vector2 position, SpriteBatch spriteBatch, SpriteFont font, Color color) : base(game)
        {
            this.position = position;
            this.color = color;
            this.spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            this.font = font;

            fps = new FramesPerSecondCounter();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            fps.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            fps.Draw(gameTime);

            spriteBatch.DrawString(font, fps.FramesPerSecond.ToString(), position, color);
        }
    }
}
