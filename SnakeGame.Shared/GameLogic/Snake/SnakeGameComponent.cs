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
        private readonly ISnake _snake;
        private readonly IGraphics2DComponent _graphicsComponent;
        private readonly IMovingCalculator _movingCalculator;
        private readonly SnakeControls _controls;
        private readonly IGameSettings _gameSettings;
        private readonly ILogger _logger;

        private Vector2 _unitVector;
        private TimeSpan _movingTime = TimeSpan.FromMilliseconds(1000d);
        private TimeSpan _movingElapsedTime = TimeSpan.Zero;
        private TimeSpan _elapsedTime;
        private TimeSpan _stepIntervalTime;
        private Direction _nextDirection;

        public SnakeGameComponent(ISnake snake, IGraphics2DComponent graphicsComponent, IMovingCalculator movingCalculator, SnakeControls controls, IGameSettings gameSettings, ILogger logger)
        {
            _snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _graphicsComponent = graphicsComponent ?? throw new ArgumentNullException(nameof(graphicsComponent));
            _movingCalculator = movingCalculator ?? throw new ArgumentNullException(nameof(movingCalculator));
            _controls = controls ?? throw new ArgumentNullException(nameof(controls));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Initialize();
        }

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public int DrawOrder { get; set; }

        public bool Visible { get; set; }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public void Initialize()
        {
            _unitVector = new Vector2(_gameSettings.TileWidth, _gameSettings.TileHeight);
            _stepIntervalTime = TimeSpan.FromMilliseconds(500);
            _nextDirection = _snake.Direction;
        }

        public void Update(GameTime gameTime)
        {
            _logger.Debug($"SnakeGameComponent.Update({gameTime.TotalGameTime}");

            Move(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            UpdateNextDirection();

            switch (_snake.State)
            {
                case SnakeState.None:
                    {
                        if (_elapsedTime >= _stepIntervalTime)
                        {
                            _elapsedTime -= _stepIntervalTime;
                            _snake.SetDirection(_nextDirection);

                            StartMoving();
                        }

                        break;
                    }
                case SnakeState.Moving:
                    {
                        Moving(gameTime);

                        break;
                    }
            }
        }

        /// <summary>
        /// Update snake's head direction by user input
        /// </summary>
        private void UpdateNextDirection()
        {
            if (InputHandler.IsKeyDown(_controls.Up) && _snake.Direction != Direction.Down)
            {
                _nextDirection = Direction.Up;
            }
            else if (InputHandler.IsKeyDown(_controls.Down) && _snake.Direction != Direction.Up)
            {
                _nextDirection = Direction.Down;
            }
            else if (InputHandler.IsKeyDown(_controls.Right) && _snake.Direction != Direction.Left)
            {
                _nextDirection = Direction.Right;
            }
            else if (InputHandler.IsKeyDown(_controls.Left) && _snake.Direction != Direction.Right)
            {
                _nextDirection = Direction.Left;
            }
        }

        private void Moving(GameTime gameTime)
        {
            _logger.Debug($"SnakeComponent.Moving(): {nameof(_movingElapsedTime)}={_movingElapsedTime}");

            UpdatePosition(gameTime);

            var head = _snake.Segments.First.Value;

            if (!head.TargetPosition.HasValue
                || head.Position == head.TargetPosition.Value)
            {
                EndMoving();
            }

            if (_movingElapsedTime >= _movingTime)
            {
                _movingElapsedTime -= _movingTime;
            }
        }

        private void StartMoving()
        {
            _logger.Debug($"SnakeComponent.StartMoving(): {nameof(_movingElapsedTime)}={_movingElapsedTime}");

            _snake.SetState(SnakeState.Moving);

            UpdateSourcePositions();
            UpdateTargetPositions();
        }

        private void EndMoving()
        {
            _logger.Debug($"SnakeComponent.EndMoving(): {nameof(_movingElapsedTime)}={_movingElapsedTime}");

            _snake.SetState(SnakeState.None);

            UpdateDirections();
        }

        public void Draw(GameTime gameTime)
        {
            _graphicsComponent.Draw(gameTime);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            _movingElapsedTime += gameTime.ElapsedGameTime;

            _logger.Debug($"SnakeComponent.UpdatePosition(): {nameof(_movingElapsedTime)}={_movingElapsedTime}");

            for (var s = _snake.Segments.First; s != null; s = s.Next)
            {
                var segment = s.Value;

                if (!segment.TargetPosition.HasValue)
                {
                    segment.SetTargetPosition(s.Previous.Value.SourcePosition.Value);
                }

                if (!segment.SourcePosition.HasValue)
                {
                    segment.SetSourcePosition(segment.Position);
                }

                if (segment.SourcePosition.HasValue
                    && segment.TargetPosition.HasValue)
                {
                    segment.MoveTo(_movingCalculator.CalculateMoving(segment.SourcePosition.Value, segment.TargetPosition.Value, _movingTime, _movingElapsedTime));
                }
            }
        }

        private void UpdateSourcePositions()
        {
            _logger.Debug($"SnakeComponent.UpdateSourcePositions(): {nameof(_movingElapsedTime)}={_movingElapsedTime}");

            foreach (var p in _snake.Segments)
            {
                p.SetSourcePosition(p.Position);
            }
        }

        private void UpdateTargetPositions()
        {
            _logger.Debug($"SnakeComponent.UpdateDestinationPositions(): {nameof(_movingElapsedTime)}={_movingElapsedTime}");

            foreach (var p in _snake.Segments)
            {
                p.SetTargetPosition(_movingCalculator.CalculateTargetPoint(p.Direction, p.Position, _unitVector));
            }
        }

        private void UpdateDirections()
        {
            for (var p = _snake.Segments.Last; p != null; p = p.Previous)
            {
                if (p.Previous != null)
                {
                    p.Value.SetDirection(p.Previous.Value.Direction);
                }
            }
        }
    }
}
