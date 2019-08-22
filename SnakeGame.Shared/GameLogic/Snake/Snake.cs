using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class Snake : ISnake
    {
        private readonly ILogger _logger;
        private readonly IGameField _gameField;
        private readonly IGameSettings _gameSettings;

        private List<ISnakeSegment> _tail;

        private Vector2 _unitVector;

        public Snake(ILogger logger, IGameField gameField, IGameSettings gameSettings)
        {
            _logger = logger;
            _gameField = gameField;
            _gameSettings = gameSettings;

            Initialize();
        }

        public ISnakeSegment Head { get; private set; }

        public IList<ISnakeSegment> Tail => _tail;

        public Direction Direction => Head.Direction;
        public SnakeState State { get; private set; }

        public void Initialize()
        {
            Head = GenerateRandomSegment();
            _tail = new List<ISnakeSegment>();

            _unitVector = new Vector2(_gameSettings.TileSize, 0);
        }

        public void Reset()
        {
            Head = GenerateRandomSegment();
            _tail.Clear();
            Grow();
        }

        public void Grow()
        {
            _logger.Debug($"Snake.Grow()");

            if (Head == null)
            {
                Head = GenerateRandomSegment();
            }
            else
            {
                var last = Tail.Any() ? Tail.LastOrDefault() : Head;

                if (last != null)
                {
                    var position = last.Position + DirectionHelper.RotateVector(_unitVector, DirectionHelper.GetOppositeDirection(last.Direction));
                    _tail.Add(new SnakeSegment(position, last.Size, last.Direction));
                }
            }
        }

        private ISnakeSegment GenerateRandomSegment()
        {
            var position = _gameField.GetRandomCell().Bounds.Center.ToVector2();
            var direction = DirectionHelper.GetRandom();
            var size = new Size2(_gameSettings.TileSize, _gameSettings.TileSize);

            return new SnakeSegment(position, size, direction);
        }

        public void SetDirection(Direction direction)
        {
            Head?.SetDirection(direction);
        }

        public void SetState(SnakeState state)
        {
            State = state;
        }
    }
}
