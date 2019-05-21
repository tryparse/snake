using Microsoft.Xna.Framework;
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
using SnakeGame.Shared.Settings;

namespace snake.GameEntities.Snake
{
    public class SnakeComponent : IGameComponent, IUpdateable
    {
        private readonly ILogger _logger;
        private readonly IGameSettings _gameSettings;

        private readonly Field _field;
        private readonly SnakeKeys _controls;
        private readonly LinkedList<SnakePart> _parts;
        private IMovingCalculator _movingCalculator;

        private Vector2 _tileSize;

        private readonly TimeSpan _stepTime;
        private TimeSpan _elapsedTime;

        private bool _enabled;
        private int _updateOrder;

        private SnakeState _state;
        private TimeSpan _movingTime = TimeSpan.FromMilliseconds(1000);
        private Vector2 targetPosition;

        private Direction _nextDirection;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public SnakeComponent(ILogger logger, IGameSettings gameSettings, Field field, Vector2 position, SnakeKeys controls, Direction direction = Direction.Right)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            _field = field ?? throw new ArgumentNullException(nameof(field));
            _controls = controls ?? throw new ArgumentNullException(nameof(controls));
            _stepTime = TimeSpan.FromSeconds(1);

            _nextDirection = direction;

            // TODO: Rework initialization
            // Should be invoke before adding part
            Initialize();

            _parts = new LinkedList<SnakePart>();
            _parts.AddFirst(new SnakePart(position, _tileSize, direction));
        }

        public LinkedList<SnakePart> Parts => _parts;

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
            _state = SnakeState.None;
            _movingCalculator = new MovingCalculator(_logger, _field);
            _tileSize = new Vector2(_gameSettings.TileWidth, _gameSettings.TileHeight);
        }

        public void Reset()
        {
            _parts.Clear();

            AddTail();

            Enabled = true;
        }

        public void AddTail(int count = 1)
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
                    newPartPosition = _movingCalculator.FindNeighbourPoint(DirectionHelper.GetOppositeDirection(tail.Direction), tail.Position, _tileSize);
                    newPartDirection = tail.Direction;
                }

                _parts.AddLast(new SnakePart(new Vector2(newPartPosition.X, newPartPosition.Y), _tileSize, newPartDirection));
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

            // TODO: moving of all snake parts

            switch (_state)
            {
                case SnakeState.None:
                    {
                        if (_elapsedTime >= _stepTime)
                        {
                            _elapsedTime -= _stepTime;
                            head.Direction = _nextDirection;

                            StartMoving();

                            targetPosition = _movingCalculator.FindNeighbourPoint(head.Direction, head.Position, _tileSize);

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
                        var newPosition = _movingCalculator.Calculate(head.Position, targetPosition, _movingTime, _elapsedTime);

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
    }
}
