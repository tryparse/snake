using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using System;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class SnakeGameComponent : ISnakeGameComponent
    {
        private readonly IGraphics2DComponent _graphicsComponent;
        private readonly ILogger _logger;

        public SnakeGameComponent(ISnake snake, IGraphics2DComponent graphicsComponent, ISnakeMovementComponent snakeMovement, ILogger logger)
        {
            Snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _graphicsComponent = graphicsComponent ?? throw new ArgumentNullException(nameof(graphicsComponent));
            SnakeMovementComponent = snakeMovement ?? throw new ArgumentNullException(nameof(snakeMovement));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Initialize();
        }

        #region IUpdatable & IDrawable

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public int DrawOrder { get; set; }

        public bool Visible { get; set; }

        public ISnake Snake { get; }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        #endregion IUpdatable & IDrawable

        #region ISnakeGameComponent

        public ISnakeMovementComponent SnakeMovementComponent { get; }

        #endregion ISnakeGameComponent

        public void Initialize()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                SnakeMovementComponent.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                _graphicsComponent.Draw(gameTime);
            }
        }

        public void Reset()
        {
            Snake.Reset();
            SnakeMovementComponent.Reset();
        }

        public void ToggleEnabled()
        {
            Enabled = !Enabled;
        }

        public void ToggleEnabled(bool enabled)
        {
            Enabled = enabled;
        }
    }
}
