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
        private readonly IGameLogger _logger;
        private readonly ISnakeMovementComponent _snakeMovementComponent;
        private readonly ISnakeEntity _snake;

        public SnakeGameComponent(ISnakeEntity snake, IGraphics2DComponent graphicsComponent, ISnakeMovementComponent snakeMovement, IGameLogger logger)
        {
            _snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _graphicsComponent = graphicsComponent ?? throw new ArgumentNullException(nameof(graphicsComponent));
            _snakeMovementComponent = snakeMovement ?? throw new ArgumentNullException(nameof(snakeMovement));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Update(GameTime gameTime)
        {
            _snakeMovementComponent.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _graphicsComponent.Draw(spriteBatch);
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            _graphicsComponent.DebugDraw(spriteBatch);
        }

        public void Reset()
        {
            _snake.Reset();
            _snakeMovementComponent.Reset();
        }

        public void ToggleEnabled(bool enabled)
        {
            throw new NotImplementedException();
        }

        public void ToggleEnabled()
        {
            
        }
    }
}
