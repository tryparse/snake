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
using MonoGame.Extended;

namespace snake.GameEntities
{
    public class Snake : IGameComponent, IUpdateable
    {
        private readonly ILogger _logger;
        private readonly Field _field;
        private readonly SnakeKeys _controls;
        private readonly List<SnakePart> _parts;

        private readonly int _stepTime;
        private int elapsedTime;

        private bool _enabled;
        private int _updateOrder;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public Snake(ILogger logger, Field field, Vector2 headPosition, SnakeKeys controls, Direction direction = Direction.Right)
        {
            this._logger = logger;
            this._field = field;
            this._controls = controls;
            this._stepTime = 1000;
            this._parts = new List<SnakePart>();
            _parts.Add(new SnakePart(headPosition, TileMetrics.Size, direction));

            AddPart();
            AddPart();
            AddPart();
        }

        public List<SnakePart> Parts => _parts;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                EnabledChanged?.Invoke(this, new EventArgs());
                _enabled = value;
            }
        }

        public int UpdateOrder
        {
            get => _updateOrder;
            set
            {
                UpdateOrderChanged?.Invoke(this, new EventArgs());
                _updateOrder = value;
            }
        }

        public void Initialize()
        {
            // Nothing to initialize
        }

        public void AddPart()
        {
            var tail = _parts.Last();

            var partPosition = FindNeighbourPoint(DirectionHelper.GetOppositeDirection(tail.Direction), tail.Position, TileMetrics.Size);

            _parts.Add(new SnakePart(new Vector2(partPosition.X, partPosition.Y), tail.Size, tail.Direction ));
        }

        public void Update(GameTime gameTime)
        {
            var head = _parts.First();

            if (InputHandler.IsKeyDown(_controls.Up) && head.Direction != Direction.Down)
            {
                head.Direction = Direction.Up;
            }
            else if (InputHandler.IsKeyDown(_controls.Down) && head.Direction != Direction.Up)
            {
                head.Direction = Direction.Down;
            }
            else if (InputHandler.IsKeyDown(_controls.Right) && head.Direction != Direction.Left)
            {
                head.Direction = Direction.Right;
            }
            else if (InputHandler.IsKeyDown(_controls.Left) && head.Direction != Direction.Right)
            {
                head.Direction = Direction.Left;
            }

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= _stepTime)
            {
                elapsedTime = elapsedTime - _stepTime;

                if (Enabled)
                {
                    MoveTail();
                    MoveHead();
                }
            }
        }

        private void MoveHead()
        {
            var head = _parts.First();

            var step = TileMetrics.Size;
            var nextPosition = FindNeighbourPoint(head.Direction, head.Position, step);

            head.Position = new Vector2(nextPosition.X, nextPosition.Y);
        }

        private Vector2 FindNeighbourPoint(Direction direction, Vector2 point, Vector2 step)
        {
            var offset = Vector2.Zero;

            switch (direction)
            {
                case Direction.Up:
                    {
                        offset.Y -= step.Y;
                        break;
                    }
                case Direction.Down:
                    {
                        offset.Y += step.Y;
                        break;
                    }
                case Direction.Right:
                    {
                        offset.X += step.X;
                        break;
                    }
                case Direction.Left:
                    {
                        offset.X -= step.X;
                        break;
                    }
            }

            var result = Vector2.Add(point, offset);

            result.X = result.X > _field.Bounds.Width ? step.X / 2 : result.X < 0 ? _field.Bounds.Width - step.X / 2 : result.X;
            result.Y = result.Y > _field.Bounds.Height ? step.Y / 2 : result.Y < 0 ? _field.Bounds.Height - step.Y / 2 : result.Y;

            return result;
        }

        private void MoveTail()
        {
            for (int i = _parts.Count - 1; i >= 1; i--)
            {
                _parts[i].Position = _parts[i - 1].Position;
                _parts[i].Direction = _parts[i - 1].Direction;
            }
        }
    }
}
