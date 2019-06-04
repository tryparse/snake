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

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class SnakeGameComponent : ISnakeGameComponent
    {
        private readonly ISnake _snake;
        private readonly IGraphics2DComponent _graphicsComponent;
        private readonly IMovingCalculator _movingCalculator;
        private readonly SnakeControls _controls;
        private readonly IGameSettings _gameSettings;
        private readonly IGameManager _gameManager;
        private readonly ILogger _logger;

        private Vector2 _unitVector;
        private TimeSpan _movingTime = TimeSpan.FromMilliseconds(500d);
        private TimeSpan _movingElapsedTime = TimeSpan.Zero;
        private Dictionary<ISnakeSegment, Vector2> _sourcePositions;
        private Dictionary<ISnakeSegment, Vector2> _destinationPositions;
        private TimeSpan _elapsedTime;
        private TimeSpan _stepIntervalTime;
        private Direction _nextDirection;

        public SnakeGameComponent(ISnake snake, IGraphics2DComponent graphicsComponent, IMovingCalculator movingCalculator, SnakeControls controls, IGameSettings gameSettings, IGameManager gameManager, ILogger logger)
        {
            _snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _graphicsComponent = graphicsComponent ?? throw new ArgumentNullException(nameof(graphicsComponent));
            _movingCalculator = movingCalculator ?? throw new ArgumentNullException(nameof(movingCalculator));
            _controls = controls ?? throw new ArgumentNullException(nameof(controls));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        }

        public void Update(GameTime gameTime)
        {
            if (InputHandler.IsKeyDown(_controls.Up) && _snake.Direction != Direction.Down)
            {
                _snake.SetDirection(Direction.Up);
            }
            else if (InputHandler.IsKeyDown(_controls.Down) && _snake.Direction != Direction.Up)
            {
                _snake.SetDirection(Direction.Down);
            }
            else if (InputHandler.IsKeyDown(_controls.Right) && _snake.Direction != Direction.Left)
            {
                _snake.SetDirection(Direction.Right);
            }
            else if (InputHandler.IsKeyDown(_controls.Left) && _snake.Direction != Direction.Right)
            {
                _snake.SetDirection(Direction.Left);
            }
        }

        private void Move(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            switch (_snake.State)
            {
                case SnakeState.None:
                    {
                        if (_elapsedTime >= _stepIntervalTime)
                        {
                            _elapsedTime -= _stepIntervalTime;

                            _snake.SetDirection(_nextDirection);

                            // TODO: check collisions and food

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

        private void Moving(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        private void StartMoving()
        {
            _snake.SetState(SnakeState.Moving);
        }

        private void EndMoving()
        {
            _snake.SetState(SnakeState.None);
        }

        public void Draw(GameTime gameTime)
        {
            _graphicsComponent.Draw(gameTime);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            _movingElapsedTime += gameTime.ElapsedGameTime;

            _logger.Debug($"SnakeComponent.UpdatePosition(): {nameof(_movingElapsedTime)}={_movingElapsedTime}");

            foreach (var p in _snake.Segments)
            {
                p.Position = _movingCalculator.Calculate(_sourcePositions[p], _destinationPositions[p], _movingTime, _movingElapsedTime);
            }
        }

        private void UpdateSourcePositions()
        {
            foreach (var p in _snake.Segments)
            {
                _sourcePositions[p] = p.Position;
            }
        }

        private void UpdateDestinationPosition()
        {
            foreach (var p in _snake.Segments)
            {
                _destinationPositions[p] = _movingCalculator.FindNeighbourPoint(p.Direction, p.Position, _unitVector);
            }
        }

        private void UpdateDirection()
        {
            for (LinkedListNode<ISnakeSegment> p = _snake.Segments.Last; p != null; p = p.Previous)
            {
                if (p.Previous != null)
                {
                    p.Value.Direction = p.Previous.Value.Direction;
                }
            }
        }
    }
}
