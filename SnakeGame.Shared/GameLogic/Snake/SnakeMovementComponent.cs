using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public interface IMovingObject
    {
        Vector2? SourcePosition { get; set; }
        Vector2? TargetPosition { get; set; }
        Vector2 CurrentPosition { get; }
        Direction CurrentDirection { get; }
    }

    public class SegmentMoving : IMovingObject
    {
        private readonly ISnakeSegment _snakeSegment;

        public Vector2? SourcePosition { get; set; }

        public Vector2? TargetPosition { get; set; }

        public Vector2 CurrentPosition { get; }

        public Direction CurrentDirection { get; }

        public SegmentMoving(ISnakeSegment segment)
        {
            _snakeSegment = segment;

            CurrentPosition = segment.Position;
            SourcePosition = segment.Position;
            CurrentDirection = segment.Direction;
        }
    }

    public class SnakeMovementTurnBased : ISnakeMovementComponent
    {
        private readonly ISnake _snake;
        private readonly IGameField _gameField;
        private readonly IGameSettings _gameSettings;
        private readonly SnakeControls _controls;

        private TimeSpan _elapsedTime;
        private TimeSpan _movingDuration;

        private readonly Vector2 _unitVector;
        private List<IMovingObject> _movingObjects;

        private Direction? _nextDirection;

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public SnakeMovementTurnBased(ISnake snake, IGameField gameField, IGameSettings gameSettings, SnakeControls controls)
        {
            _snake = snake;
            _gameField = gameField;
            _gameSettings = gameSettings;
            _controls = controls;

            _movingDuration = TimeSpan.FromMilliseconds(_gameSettings.DefaultMoveIntervalTime);

            _movingObjects = new List<IMovingObject>();
            _movingObjects.Add(new SegmentMoving(_snake.Head));
            _movingObjects.AddRange(_snake.Tail.Select(x => new SegmentMoving(x)));

            _unitVector = Vector2.Multiply(Vector2.UnitX, _gameSettings.TileSize);
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            UpdateNextDirection();

            if (_elapsedTime >= _movingDuration)
            {
                if (_nextDirection.HasValue)
                {
                    _snake.Head.SetDirection(_nextDirection.Value);
                }

                foreach (var mo in _movingObjects)
                {
                    mo.TargetPosition = mo.CurrentPosition + DirectionHelper.RotateVector(_unitVector, Direction.Up);
                }

                _elapsedTime -= _movingDuration;
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

        private Vector2 GetTargetPosition()
        {
            throw new NotImplementedException();
        }
    }
}
