using Microsoft.Xna.Framework;
using snake.Common;
using snake.GameComponents;
using snake.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.GameLogic.GameField;

namespace snake.GameEntities.Snake
{
    public class SnakeComponent : IGameComponent, IUpdateable
    {
        private readonly ILogger _logger;
        private readonly Field _field;
        private readonly SnakeKeys _controls;
        private readonly List<SnakePart> _parts;
        private readonly MovingCalculator _movingCalculator;

        private readonly TimeSpan _stepTime;
        private TimeSpan _elapsedTime;

        private bool _enabled;
        private int _updateOrder;

        private SnakeState _state;
        private TimeSpan _transitionTime = TimeSpan.FromMilliseconds(1000);
        private Vector2 targetPosition;

        private Direction _nextDirection;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public SnakeComponent(ILogger logger, Field field, Vector2 position, SnakeKeys controls, Direction direction = Direction.Right)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._field = field ?? throw new ArgumentNullException(nameof(field));
            this._controls = controls;
            this._stepTime = TimeSpan.FromSeconds(1);
            this._parts = new List<SnakePart>();
            _parts.Add(new SnakePart(position, TileMetrics.Size, direction));

            _nextDirection = direction;

            //AddPart(4);

            _state = SnakeState.None;
            _movingCalculator = new MovingCalculator(_logger, _field);
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

        public void Reset()
        {
            _parts.Clear();

            AddPart();

            Enabled = true;
        }

        public void AddPart(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                var tail = _parts.LastOrDefault();

                Vector2 newPartPosition;
                var newPartDirection = default(Direction);

                if (tail == null)
                {
                    newPartPosition = _field.Cells[0, 0].Bounds.Center.ToVector2();
                    newPartDirection = DirectionHelper.GetRandom();
                }
                else
                {
                    newPartPosition = _movingCalculator.FindNeighbourPoint(DirectionHelper.GetOppositeDirection(tail.Direction), tail.Position, TileMetrics.Size);
                    newPartDirection = tail.Direction;
                }

                _parts.Add(new SnakePart(new Vector2(newPartPosition.X, newPartPosition.Y), TileMetrics.Size, newPartDirection));
            }
        }

        public void Update(GameTime gameTime)
        {
            var head = _parts.First();

            if (InputHandler.IsKeyDown(_controls.Up) && head.Direction != Direction.Down)
            {
                _nextDirection = Direction.Up;
            }
            else if (InputHandler.IsKeyDown(_controls.Down) && head.Direction != Direction.Up)
            {
                _nextDirection = Direction.Down;
            }
            else if (InputHandler.IsKeyDown(_controls.Right) && head.Direction != Direction.Left)
            {
                _nextDirection = Direction.Right;
            }
            else if (InputHandler.IsKeyDown(_controls.Left) && head.Direction != Direction.Right)
            {
                _nextDirection = Direction.Left;
            }

            Move(gameTime, head);
        }

        private void Move(GameTime gameTime, SnakePart head)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            switch (_state)
            {
                case SnakeState.None:
                    {
                        if (_elapsedTime >= _stepTime)
                        {
                            _elapsedTime -= _stepTime;
                            head.Direction = _nextDirection;

                            StartMoving();

                            targetPosition = _movingCalculator.FindNeighbourPoint(head.Direction, head.Position, TileMetrics.Size);

                            if (GameManager.CheckSnakeCollision(this))
                            {
                                GameManager.NewGame();
                            }

                            _state = SnakeState.Moving;
                        }

                        break;
                    }
                case SnakeState.Moving:
                    {
                        var newPosition = _movingCalculator.Calculate(head.Position, targetPosition, _transitionTime, _elapsedTime);

                        head.Position = newPosition;

                        if (head.Position == targetPosition)
                        {
                            _state = SnakeState.None;
                        }

                        break;
                    }
            }
        }

        private void StartMoving()
        {
            _state = SnakeState.Moving;
        }

        private void MoveHead()
        {
            var head = _parts.First();

            var step = TileMetrics.Size;
            var nextPosition = _movingCalculator.FindNeighbourPoint(head.Direction, head.Position, step);

            head.Position = new Vector2(nextPosition.X, nextPosition.Y);
        }

        private void MoveTail()
        {
            for (int i = _parts.Count - 1; i >= 1; i--)
            {
                _parts[i].Position = _parts[i - 1].Position;
                _parts[i].Direction = _parts[i - 1].Direction;
            }
        }

        private bool CheckHeadCollision()
        {
            var head = _parts.First();

            var tail = _parts.Skip(1);

            foreach (var part in tail)
            {
                if (head.Bounds.Intersects(part.Bounds))
                {
                    _enabled = false;
                    return true;
                }
            }

            return false;
        }
    }
}
