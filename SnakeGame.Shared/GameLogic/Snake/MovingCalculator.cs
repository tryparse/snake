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
        private readonly IGameField _gameField;

        public MovingCalculator(ILogger logger, IGameField gameField)
        {
            _logger = logger;
            _gameField = gameField;
        }

        public Vector2 Calculate(Vector2 currentPosition, Vector2 targetPosition, TimeSpan transitionTime, TimeSpan elapsedTransitionTime)
        {
            var amount = (float)elapsedTransitionTime.TotalMilliseconds / (float)transitionTime.TotalMilliseconds;

            amount = Math.Min(amount, 1);

            var resultPosition = Vector2.Lerp(currentPosition, targetPosition, amount);

            return resultPosition;
        }

        public Vector2 FindNeighbourPoint(Direction direction, Vector2 point, Vector2 step)
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

            //result = CheckBoundaries(step, result);

            return result;
        }

        public Vector2 CheckBoundaries(Vector2 step, Vector2 result)
        {
            _logger.Debug($"MovingCalculator.CheckBoundaries(): input={result}");

            // If on the X axis we are left the field on the right side
            if (result.X > _gameField.Bounds.Width - (step.X / 2))
            {
                result.X = step.X / 2;
            }
            // or on the left side
            else if (result.X < step.X / 2)
            {
                result.X = _gameField.Bounds.Width - step.X / 2;
            }

            if (result.Y > _gameField.Bounds.Height - (step.Y / 2))
            {
                result.Y = step.Y / 2;
            }
            else if (result.Y < step.Y / 2)
            {
                result.Y = _gameField.Bounds.Height - step.Y / 2;
            }

            _logger.Debug($"MovingCalculator.CheckBoundaries(): output={result}");

            return result;
        }
    }
}
