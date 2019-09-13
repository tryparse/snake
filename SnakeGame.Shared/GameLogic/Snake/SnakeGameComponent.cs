using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class SnakeGameComponent : ISnakeGameComponent
    {
        private readonly IGraphics2DComponent _graphicsComponent;
        private readonly ILogger _logger;

        public SnakeGameComponent(ISnakeEntity snake, IGraphics2DComponent graphicsComponent, ISnakeMovementComponent snakeMovement, ILogger logger)
        {
            Snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _graphicsComponent = graphicsComponent ?? throw new ArgumentNullException(nameof(graphicsComponent));
            SnakeMovementComponent = snakeMovement ?? throw new ArgumentNullException(nameof(snakeMovement));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ISnakeEntity Snake { get; }

        #region ISnakeGameComponent

        public ISnakeMovementComponent SnakeMovementComponent { get; }

        #endregion ISnakeGameComponent

        public void ToggleEnabled(bool enabled)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            SnakeMovementComponent.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _graphicsComponent.Draw(spriteBatch, gameTime);
        }

        public void Reset()
        {
            Snake.Reset();
            SnakeMovementComponent.Reset();
        }

        public void ToggleEnabled()
        {
            
        }
    }
}
