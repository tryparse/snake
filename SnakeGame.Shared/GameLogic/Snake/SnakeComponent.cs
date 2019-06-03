﻿using Microsoft.Xna.Framework;
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
using SnakeGame.Shared.GameLogic.GameField.Interfaces;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class SnakeComponent : IGameComponent, IUpdateable
    {
        private readonly ILogger _logger;
        private readonly IGameSettings _gameSettings;
        private readonly IGameManager _gameManager;

        private readonly IGameField _gameField;
        private readonly SnakeControls _controls;
        private readonly LinkedList<SnakePart> _parts;
        private IMovingCalculator _movingCalculator;

        private Vector2 _unitVector;

        private readonly TimeSpan _stepTime;
        private TimeSpan _elapsedTime;

        private bool _enabled;
        private int _updateOrder;

        private SnakeState _state;
        private TimeSpan _movingTime = TimeSpan.FromMilliseconds(1000d);
        private TimeSpan _movingElapsedTime = TimeSpan.Zero;
        private Dictionary<SnakePart, Vector2> _sourcePositions;
        private Dictionary<SnakePart, Vector2> _destinationPositions;

        private Direction _nextDirection;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public SnakeComponent(ILogger logger, IGameSettings gameSettings, IGameManager gameManager, IGameField gameField, Vector2 position, SnakeControls controls, Direction direction = Direction.Right)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
            _gameField = gameField ?? throw new ArgumentNullException(nameof(gameField));
            _controls = controls ?? throw new ArgumentNullException(nameof(controls));

            _stepTime = TimeSpan.FromMilliseconds(1000d);

            _nextDirection = direction;

            // TODO: Rework initialization
            // Should be invoke before adding part
            Initialize();

            _parts = new LinkedList<SnakePart>();
            _parts.AddFirst(new SnakePart(position, _unitVector, direction));
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
            _movingCalculator = new MovingCalculator(_logger, _gameField);
            _unitVector = new Vector2(_gameSettings.TileWidth, _gameSettings.TileHeight);

            _sourcePositions = new Dictionary<SnakePart, Vector2>();
            _destinationPositions = new Dictionary<SnakePart, Vector2>();
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
                    newPartPosition = _gameField.Cells[0, 0].Bounds.Center.ToVector2();
                    newPartDirection = DirectionHelper.GetRandom();
                }
                else
                {
                    newPartPosition = _movingCalculator.FindNeighbourPoint(DirectionHelper.GetOppositeDirection(tail.Direction), tail.Position, _unitVector);
                    newPartDirection = tail.Direction;
                }

                _parts.AddLast(new SnakePart(new Vector2(newPartPosition.X, newPartPosition.Y), _unitVector, newPartDirection));
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
            //_elapsedUpdateTime += gameTime.ElapsedGameTime;

            // TODO: moving of all snake parts

            switch (_state)
            {
                case SnakeState.None:
                    {
                        if (_elapsedTime >= _stepTime)
                        {
                            _elapsedTime -= _stepTime;
                            head.Direction = _nextDirection;

                            //if (_gameManager.CheckSnakeCollision(this))
                            //{
                            //    //Enabled = false;
                            //    //_gameManager.NewGame(this);
                            //}

                            //if (_gameManager.CheckFoodEating(this))
                            //{

                            //}

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

        private void StartMoving()
        {
            _logger.Debug($"SnakeComponent.StartMoving({_elapsedTime.ToString()})");

            _state = SnakeState.Moving;

            UpdateSourcePositions();
            UpdateDestinationPosition();
        }

        private void Moving(GameTime gameTime)
        {
            UpdatePosition(gameTime);

            if (_movingElapsedTime >= _movingTime)
            {
                _movingElapsedTime -= _movingTime;
            }

            var head = _parts.First.Value;

            if (head.Position == _destinationPositions[head])
            {
                EndMoving();
            }
        }

        private void EndMoving()
        {
            _logger.Debug($"SnakeComponent.EndMoving({_elapsedTime.ToString()})");

            _state = SnakeState.None;

            _sourcePositions.Clear();
            _destinationPositions.Clear();

            UpdateDirection();
        }

        private void UpdatePosition(GameTime gameTime)
        {
            _movingElapsedTime += gameTime.ElapsedGameTime;

            _logger.Debug($"SnakeComponent.UpdatePosition(): {nameof(_movingElapsedTime)}={_movingElapsedTime}");

            foreach (var p in _parts)
            {
                p.Position = _movingCalculator.Calculate(_sourcePositions[p], _destinationPositions[p], _movingTime, _movingElapsedTime);
            }
        }

        private void UpdateSourcePositions()
        {
            foreach (var p in _parts)
            {
                _sourcePositions[p] = p.Position;
            }
        }

        private void UpdateDestinationPosition()
        {
            foreach (var p in _parts)
            {
                _destinationPositions[p] = _movingCalculator.FindNeighbourPoint(p.Direction, p.Position, _unitVector);
            }
        }

        private void UpdateDirection()
        {
            for (LinkedListNode<SnakePart> p = _parts.Last; p != null; p = p.Previous)
            {
                if (p.Previous != null)
                {
                    p.Value.Direction = p.Previous.Value.Direction;
                }
            }
        }
    }
}
