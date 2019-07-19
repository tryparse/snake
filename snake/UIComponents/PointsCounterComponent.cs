using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic;
using SnakeGame.Shared.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.UIComponents
{
    public class PointsCounterComponent : DrawableGameComponent
    {
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly IGamePoints gamePoints;
        private readonly Vector2 _position;

        public PointsCounterComponent(Game game, Vector2 position, IGraphicsSystem graphicsSystem, IGamePoints gamePoints) : base(game)
        {
            _position = position;
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            this.gamePoints = gamePoints;
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsSystem.SpriteBatch.DrawString(_graphicsSystem.SpriteFont, $"Points: {gamePoints.Points}", _position, Color.Green);
        }
    }
}
