using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class SnakeGameComponent : ISnakeGameComponent
    {
        private readonly IGraphics2DComponent _graphicsComponent;
        private readonly ISnakeMovementComponent _movement;
        private readonly ILogger _logger;

        public SnakeGameComponent(IGraphics2DComponent graphicsComponent, ISnakeMovementComponent snakeMovement, ILogger logger)
        {
            _graphicsComponent = graphicsComponent ?? throw new ArgumentNullException(nameof(graphicsComponent));
            _movement = snakeMovement ?? throw new ArgumentNullException(nameof(snakeMovement));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Initialize();
        }

        #region IUpdatable & IDrawable

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public int DrawOrder { get; set; }

        public bool Visible { get; set; }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        #endregion IUpdatable & IDrawable

        public void Initialize()
        {
        }

        public void Update(GameTime gameTime)
        {
            _logger.Debug($"SnakeGameComponent.Update({gameTime.TotalGameTime}");

            _movement.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _graphicsComponent.Draw(gameTime);
        }
    }
}
