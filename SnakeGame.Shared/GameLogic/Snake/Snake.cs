using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class Snake : ISnake
    {
        private readonly ILogger _logger;
        private readonly IGameField _gameField;
        private readonly IMovingCalculator _movingCalculator;
        private readonly IGameSettings _gameSettings;

        private ISnakeSegment _head;
        private readonly List<ISnakeSegment> tail;

        private readonly Random _random;

        public Snake(ILogger logger, IGameField gameField, IMovingCalculator movingCalculator, IGameSettings gameSettings)
        {
            _logger = logger;
            _gameField = gameField;
            _movingCalculator = movingCalculator;
            _gameSettings = gameSettings;

            tail = new List<ISnakeSegment>();

            _random = new Random();
        }

        public ISnakeSegment Head => _head;
        public IEnumerable<ISnakeSegment> Tail => tail;

        public Direction Direction => Head.Direction;
        public SnakeState State { get; private set; }

        public void Reset()
        {
            _head = null;
            tail.Clear();

            AddSegments(2);
        }

        public void AddSegments(uint count)
        {
            _logger.Debug($"Snake.AddSegments({count})");

            for (var i = 0; i < count; i++)
            {
                if (_head == null)
                {
                    _head = GenerateRandomSegment();
                }
                else
                {
                    // TODO: tail adding
                }
            }
        }

        private ISnakeSegment GenerateRandomSegment()
        {
            var x = _random.Next(1, _gameField.Columns - 1);
            var y = _random.Next(1, _gameField.Rows - 1);

            var position = _gameField.Cells[x, y].Bounds.Center.ToVector2();
            var direction = DirectionHelper.GetRandom();
            var size = new Size2(_gameSettings.TileSize, _gameSettings.TileSize);

            return new SnakeSegment(position, size, direction);
        }

        public void SetDirection(Direction direction)
        {
            if (_head != null)
            {
                _head.SetDirection(direction);
            }
        }

        public void SetState(SnakeState state)
        {
            State = state;
        }
    }
}
