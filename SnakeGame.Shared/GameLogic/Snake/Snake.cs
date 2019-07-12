using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class Snake : ISnake
    {
        private readonly ILogger _logger;
        private readonly IGameField _gameField;
        private readonly IGameSettings _gameSettings;

        private ISnakeSegment _head;
        private List<ISnakeSegment> _tail;

        private readonly Random _random;

        private int growSegments = 0;

        public Snake(ILogger logger, IGameField gameField, IMovingCalculator movingCalculator, IGameSettings gameSettings)
        {
            _logger = logger;
            _gameField = gameField;
            _gameSettings = gameSettings;

            Initialize();

            _random = new Random();
        }

        public ISnakeSegment Head => _head;
        public IList<ISnakeSegment> Tail => _tail;

        public Direction Direction => Head.Direction;
        public SnakeState State { get; private set; }

        public void Initialize()
        {
            _head = GenerateRandomSegment();
            _tail = new List<ISnakeSegment>();
        }

        public void Reset()
        {
            _head = GenerateRandomSegment();
            _tail.Clear();
        }

        public void Grow()
        {
            _logger.Debug($"Snake.Grow()");

            if (_head == null)
            {
                _head = GenerateRandomSegment();
            }
            else
            {
                var last = Tail.Any() ? Tail.LastOrDefault() : Head;

                if (last != null)
                {
                    //var position = last. .CurrentPosition + DirectionHelper.RotateVector(_unitVector, mo.CurrentDirection);
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
