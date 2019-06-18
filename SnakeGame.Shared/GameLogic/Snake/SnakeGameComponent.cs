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
        private readonly IGameField _gameField;

        private Vector2 _unitVector;
        private TimeSpan _movingTime = TimeSpan.FromMilliseconds(1000d);
        private TimeSpan _movingElapsedTime = TimeSpan.Zero;
        private Dictionary<ISnakeSegment, Vector2> _sourcePositions;
        private Dictionary<ISnakeSegment, Vector2> _destinationPositions;
        private TimeSpan _elapsedTime;
        private TimeSpan _stepIntervalTime;
        private Direction _nextDirection;

        public SnakeGameComponent(ISnake snake, IGraphics2DComponent graphicsComponent, IMovingCalculator movingCalculator, SnakeControls controls, IGameSettings gameSettings, IGameManager gameManager, ILogger logger, IGameField gameField)
        {
            _snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _graphicsComponent = graphicsComponent ?? throw new ArgumentNullException(nameof(graphicsComponent));
            _movingCalculator = movingCalculator ?? throw new ArgumentNullException(nameof(movingCalculator));
            _controls = controls ?? throw new ArgumentNullException(nameof(controls));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _gameField = gameField ?? throw new ArgumentNullException(nameof(gameField));

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
            _sourcePositions = new Dictionary<ISnakeSegment, Vector2>();
            _destinationPositions = new Dictionary<ISnakeSegment, Vector2>();
            _nextDirection = _snake.Direction;
        }

        public void Update(GameTime gameTime)
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

            Move(gameTime);
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
            UpdatePosition(gameTime);

            if (_movingElapsedTime >= _movingTime)
            {
                _movingElapsedTime -= _movingTime;
            }

            var head = _snake.Segments.First.Value;

            if (head.Position == _destinationPositions[head])
            {
                EndMoving();
            }
        }

        private void StartMoving()
        {
            _snake.SetState(SnakeState.Moving);

            UpdateSourcePositions();
            UpdateDestinationPositions();
        }

        private void EndMoving()
        {
            _snake.SetState(SnakeState.None);

            _sourcePositions.Clear();
            _destinationPositions.Clear();

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

            foreach (var p in _snake.Segments)
            {
                p.MoveTo(_movingCalculator.Calculate(_sourcePositions[p], _destinationPositions[p], _movingTime, _movingElapsedTime));

                var rightBorder = _gameField.Bounds.Right + _unitVector.X / 2;
                var leftBorder = _gameField.Bounds.Left - _unitVector.X / 2;

                if (p.Position.X > rightBorder
                    || (p.Position.X == rightBorder && p.Direction != Direction.Right))
                {
                    var newX = leftBorder;
                    var newPosition = new Vector2(newX, p.Position.Y);

                    var newSourcePosition = new Vector2(_unitVector.X * .5f, p.Position.Y);
                    var newDestinationPosition = _movingCalculator.FindNeighbourPoint(p.Direction, newSourcePosition, _unitVector);

                    _sourcePositions[p] = newSourcePosition;
                    _destinationPositions[p] = newDestinationPosition;
                    p.MoveTo(newPosition);
                }

                if (p.Position.X < leftBorder
                    || (p.Position.X == leftBorder && p.Direction != Direction.Left))
                {
                    var newX = rightBorder;
                    var newPosition = new Vector2(newX, p.Position.Y);

                    var newSourcePosition = new Vector2(_gameField.Bounds.Width - _unitVector.X / 2, p.Position.Y);
                    var newDestinationPosition = _movingCalculator.FindNeighbourPoint(p.Direction, newSourcePosition, _unitVector);

                    _sourcePositions[p] = newSourcePosition;
                    _destinationPositions[p] = newDestinationPosition;
                    p.MoveTo(newPosition);
                }

                var topBorder = _gameField.Bounds.Top - _unitVector.Y / 2;
                var bottomBorder = _gameField.Bounds.Bottom + _unitVector.Y / 2;

                if (p.Position.Y < topBorder
                    || (p.Position.Y == topBorder && p.Direction != Direction.Up))
                {
                    var newY = bottomBorder;
                    var newPosition = new Vector2(p.Position.X, newY);

                    var newSourcePosition = newPosition;
                    var newDestinationPosition = _movingCalculator.FindNeighbourPoint(p.Direction, newSourcePosition, _unitVector);

                    _sourcePositions[p] = newSourcePosition;
                    _destinationPositions[p] = newDestinationPosition;
                    p.MoveTo(newPosition);
                }

                if (p.Position.Y > bottomBorder
                    || (p.Position.Y == bottomBorder && p.Direction != Direction.Down))
                {
                    var newY = topBorder;
                    var newPosition = new Vector2(p.Position.X, newY);

                    var newSourcePosition = newPosition;
                    var newDestinationPosition = _movingCalculator.FindNeighbourPoint(p.Direction, newSourcePosition, _unitVector);

                    _sourcePositions[p] = newSourcePosition;
                    _destinationPositions[p] = newDestinationPosition;
                    p.MoveTo(newPosition);
                }
            }
        }

        private void UpdateSourcePositions()
        {
            foreach (var p in _snake.Segments)
            {
                _sourcePositions[p] = p.Position;
            }
        }

        private void UpdateDestinationPositions()
        {
            foreach (var p in _snake.Segments)
            {
                _destinationPositions[p] = _movingCalculator.FindNeighbourPoint(p.Direction, p.Position, _unitVector);
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
