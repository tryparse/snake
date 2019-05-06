using Microsoft.Xna.Framework;
using snake.Common;
using snake.Configuration;
using snake.GameComponents;
using snake.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameEntities
{
    public class Snake : IGameComponent, IUpdateable
    {
        private readonly ILogger _logger;
        private readonly Field _field;
        private Cell _headCell;
        private readonly SnakeKeys _controls;
        private readonly IEnumerable<Vector2> _tail;

        private SnakeDirection _currentDirection;
        private SnakeDirection _nextDirection;

        private readonly int stepTime;
        private int elapsedTime;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public Snake(ILogger logger, Field field, Cell headCell, SnakeKeys controls, SnakeDirection direction = SnakeDirection.Right)
        {
            this._logger = logger;
            this._field = field;
            this._headCell = headCell;
            this._tail = new HashSet<Vector2>();
            this._currentDirection = direction;
            this._nextDirection = direction;
            this._controls = controls;
            this.stepTime = 1000;
        }

        public Vector2 Position => _headCell.Bounds.Center.ToVector2();

        public Rectangle Bounds => _headCell.Bounds;

        public SnakeDirection CurrentDirection => _currentDirection;

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public void AddTail()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
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
                elapsedTime = elapsedTime - stepTime;

                if (Enabled)
                {
                    Point step = _headCell.Indices;

                    switch (_nextDirection)
                    {
                        case SnakeDirection.Up:
                            {
                                step.Y--;
                                break;
                            }
                        case SnakeDirection.Down:
                            {
                                step.Y++;
                                break;
                            }
                        case SnakeDirection.Right:
                            {
                                step.X++;
                                break;
                            }
                        case SnakeDirection.Left:
                            {
                                step.X--;
                                break;
                            }
                    }

                    step.X = step.X > _field.LengthX - 1 ? 0 : step.X < 0 ? _field.LengthX - 1 : step.X;
                    step.Y = step.Y > _field.LengthY - 1 ? 0 : step.Y < 0 ? _field.LengthY - 1 : step.Y;

                    _headCell = _field.Cells[step.X, step.Y];
                }

                _currentDirection = _nextDirection;
            }
        }

        public string GetDebugText()
        {
            return $"{nameof(_currentDirection)}={_currentDirection}\n{nameof(_nextDirection)}={_nextDirection}";
        }

        public void Initialize()
        {
            // Nothing to initialize
        }
    }
}
