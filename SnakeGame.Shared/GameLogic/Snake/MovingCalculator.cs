using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.GameLogic.GameField;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class MovingCalculator : IMovingCalculator
    {
        private readonly ILogger _logger;
        private readonly Field _field;

        public MovingCalculator(ILogger logger, Field field)
        {
            _logger = logger;
            _field = field;
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

            // TODO: rewrite this
            result.X = result.X > _field.Bounds.Width ? step.X / 2 : result.X < 0 ? _field.Bounds.Width - step.X / 2 : result.X;
            result.Y = result.Y > _field.Bounds.Height ? step.Y / 2 : result.Y < 0 ? _field.Bounds.Height - step.Y / 2 : result.Y;

            return result;
        }
    }
}
