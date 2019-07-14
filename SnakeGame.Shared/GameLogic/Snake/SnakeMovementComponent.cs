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
    public class SnakeMovementTurnBased : ISnakeMovementComponent
    {
        private readonly ISnake _snake;
        private readonly IGameField _gameField;
        private readonly IGameSettings _gameSettings;
        private readonly SnakeControls _controls;

        private TimeSpan _elapsedTime;
        private TimeSpan _movingInterval;

        private readonly Vector2 _unitVector;

        private Direction? _nextDirection;

        #region IUpdateable members

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion

        public SnakeMovementTurnBased(ISnake snake, IGameField gameField, IGameSettings gameSettings, SnakeControls controls)
        {
            _snake = snake;
            _gameField = gameField;
            _gameSettings = gameSettings;
            _controls = controls;

            _movingInterval = TimeSpan.FromMilliseconds(_gameSettings.DefaultMoveIntervalTime);

            _unitVector = Vector2.Multiply(Vector2.UnitX, _gameSettings.TileSize);
        }

        public void Update(GameTime gameTime)
        {
            UpdateNextDirection();

            switch (_snake.State)
            {
                case SnakeState.None:
                    {
                        _elapsedTime += gameTime.ElapsedGameTime;

                        if (_elapsedTime >= _movingInterval)
                        {
                            if (_nextDirection.HasValue)
                            {
                                var head = _snake.Head;

                                if (head != null)
                                {
                                    head.SetDirection(_nextDirection.Value);
                                }
                            }

                            _snake.SetState(SnakeState.Moving);

                            _elapsedTime -= _movingInterval;
                        }

                        break;
                    }
                case SnakeState.Moving:
                    {
                        for (int i = _snake.Tail.Count - 1; i > 1; i++)
                        {
                            var nextMove = _snake.Tail[i - 1].Position;
                            _snake.Tail[i].MoveTo(nextMove);
                        }

                        var next = _snake.Head.Position + DirectionHelper.RotateVector(_unitVector, _snake.Head.Direction);
                        _snake.Head.MoveTo(next);
                        _snake.SetState(SnakeState.None);

                        break;
                    }
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
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
    }
}
