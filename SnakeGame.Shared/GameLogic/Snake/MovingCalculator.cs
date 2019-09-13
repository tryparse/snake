using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.GameLogic.GameField;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class MovingCalculator : IMovingCalculator
    {
        private readonly ILogger _logger;
        private readonly IGameFieldEntity _gameField;

        public MovingCalculator(ILogger logger, IGameFieldEntity gameField)
        {
            _logger = logger;
            _gameField = gameField;
        }

        public Vector2 CalculateMoving(Vector2 currentPosition, Vector2 targetPosition, TimeSpan transitionTime, TimeSpan elapsedTransitionTime)
        {
            var amount = (float)elapsedTransitionTime.TotalMilliseconds / (float)transitionTime.TotalMilliseconds;

            amount = Math.Min(amount, 1);

            var resultPosition = Vector2.Lerp(currentPosition, targetPosition, amount);

            return resultPosition;
        }

        public Vector2 CalculateTargetPosition(Direction direction, Vector2 position, Vector2 step)
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

            var result = Vector2.Add(position, offset);

            return result;
        }
    }
}
