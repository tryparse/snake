using Microsoft.Xna.Framework;
using NLog;
using snake.Common;
using snake.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameEntities
{
    public class Snake
    {
        private readonly ILogger _logger;
        private readonly Field _field;
        private Cell _headCell;
        private readonly SnakeControls _controls;
        private IEnumerable<Vector2> _tail;

        private SnakeDirection _currentDirection;
        private SnakeDirection _nextDirection;

        private int stepTime;
        private int elapsedTime;

        private bool _isEnabled;

        public Snake(ILogger logger, Field field, Cell headCell, SnakeControls controls, SnakeDirection direction = SnakeDirection.Right)
        {
            this._logger = logger;
            this._field = field;
            this._headCell = headCell;
            this._tail = new HashSet<Vector2>();
            this._currentDirection = direction;
            this._nextDirection = direction;
            this._controls = controls;
            this.stepTime = 1000;
            _isEnabled = true;
        }

        public Vector2 Position => _headCell.Bounds.Center.ToVector2();

        public Rectangle Bounds => _headCell.Bounds;

        public SnakeDirection CurrentDirection => _currentDirection;

        public void AddTail()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if (InputHandler.IsKeyPressed(_controls.Pause))
            {
                _isEnabled = !_isEnabled;
            }

            if (InputHandler.IsKeyDown(_controls.Up) && _currentDirection != SnakeDirection.Down)
            {
                _nextDirection = SnakeDirection.Up;
            }
            else if (InputHandler.IsKeyDown(_controls.Down) && _currentDirection != SnakeDirection.Up)
            {
                _nextDirection = SnakeDirection.Down;
            }
            else if (InputHandler.IsKeyDown(_controls.Right) && _currentDirection != SnakeDirection.Left)
            {
                _nextDirection = SnakeDirection.Right;
            }
            else if (InputHandler.IsKeyDown(_controls.Left) && _currentDirection != SnakeDirection.Right)
            {
                _nextDirection = SnakeDirection.Left;
            }
            
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= stepTime)
            {
                //_logger.Debug($"Snake.Update({gameTime.TotalGameTime}) step");
                //_logger.Debug($"{nameof(_headCell)}={_headCell.Indices}");
                //_logger.Debug($"{nameof(_currentDirection)}={_currentDirection}; {nameof(_nextDirection)}={_nextDirection}");
                //_logger.Debug($"{nameof(Position)}={Position}");
                //_logger.Debug(Environment.NewLine);

                elapsedTime = elapsedTime - stepTime;

                //if (_isEnabled)
                //{
                //    var step = _headCell.Indices;

                //    switch (_nextDirection)
                //    {
                //        case SnakeDirection.Up:
                //            {
                //                step.Y--;
                //                break;
                //            }
                //        case SnakeDirection.Down:
                //            {
                //                step.Y++;
                //                break;
                //            }
                //        case SnakeDirection.Right:
                //            {
                //                step.X++;
                //                break;
                //            }
                //        case SnakeDirection.Left:
                //            {
                //                step.X--;
                //                break;
                //            }
                //    }

                //    step.X = step.X > _field.Width - 1 ? 0 : step.X < 0 ? _field.Width - 1 : step.X;
                //    step.Y = step.Y > _field.Height - 1 ? 0 : step.Y < 0 ? _field.Height - 1 : step.Y;

                //    _headCell = _field.Cells[step.X, step.Y];
                //}

                _currentDirection = _nextDirection;
            }
        }

        public string GetDebugText()
        {
            return $"{nameof(_currentDirection)}={_currentDirection}\n{nameof(_nextDirection)}={_nextDirection}";
        }
    }
}
