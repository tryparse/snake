using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Settings;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class Snake : ISnake
    {
        private readonly IGameField _gameField;
        private readonly IMovingCalculator _movingCalculator;
        private readonly IGameSettings _gameSettings;
        private readonly Vector2 _unitVector;

        public Snake(IGameField gameField, IMovingCalculator movingCalculator, IGameSettings gameSettings)
        {
            _gameField = gameField;
            _movingCalculator = movingCalculator;
            _gameSettings = gameSettings;

            _unitVector = new Vector2(_gameSettings.TileWidth, gameSettings.TileHeight);
            Segments = new LinkedList<ISnakeSegment>();
        }

        public LinkedList<ISnakeSegment> Segments { get; }
        public Direction Direction => Segments.First.Value.Direction;
        public SnakeState State { get; private set; }

        public void Reset()
        {
            Segments.Clear();

            AddSegments(1);
        }

        public void AddSegments(uint count)
        {
            for (var i = 0; i < count; i++)
            {
                var tail = Segments.LastOrDefault();

                Vector2 newSegmentPosition;

                var newSegmentDirection = default(Direction);

                if (tail == null)
                {
                    newSegmentPosition = _gameField.Cells[3, 3].Bounds.Center.ToVector2();
                    newSegmentDirection = DirectionHelper.GetRandom();
                }
                else
                {
                    newSegmentPosition =
                        _movingCalculator.CalculateTargetPoint(DirectionHelper.GetOppositeDirection(tail.Direction),
                            tail.Position, _unitVector);
                    newSegmentDirection = tail.Direction;
                }

                Segments.AddLast(new SnakeSegment(newSegmentPosition, _unitVector, newSegmentDirection));
            }
        }

        public void SetDirection(Direction direction)
        {
            if (Segments.Count > 0)
            {
                Segments.First.Value.SetDirection(direction);
            }
        }

        public void SetState(SnakeState state)
        {
            State = state;
        }
    }
}
