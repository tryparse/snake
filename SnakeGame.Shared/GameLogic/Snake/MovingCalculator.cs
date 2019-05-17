using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Shared.Logging;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class MovingCalculator
    {
        private readonly ILogger _logger;

        public MovingCalculator(ILogger logger)
        {
            _logger = logger;
        }

        public Vector2 Calculate(Direction direction, Vector2 currentPosition, Vector2 movingStep, TimeSpan transitionTime, TimeSpan elapsedTransitionTime)
        {
            _logger.Debug("MovingCalculator.Calculate");

            var offset = Vector2.Divide(movingStep, (float)elapsedTransitionTime.TotalMilliseconds / (float)transitionTime.TotalMilliseconds);

            _logger.Debug($"currentPosition={currentPosition}");
            _logger.Debug($"movingStep={movingStep}");
            _logger.Debug($"transitionTime={transitionTime}");
            _logger.Debug($"elapsedTransitionTime={ elapsedTransitionTime}");
            _logger.Debug($"offset={offset}");

            var movingOffset = Vector2.Zero;

            var isHorizontalMoving = direction == Direction.Left || direction == Direction.Right;

            if (isHorizontalMoving)
            {
                movingOffset.X = offset.X;

                if (direction == Direction.Left)
                {
                    movingOffset.X *= -1;
                }
            }
            else
            {
                movingOffset.Y = offset.Y;

                if (direction == Direction.Up)
                {
                    movingOffset.Y *= -1;
                }
            }

            _logger.Debug($"movingOffset={movingOffset}");

            return Vector2.Add(currentPosition, movingOffset);
        }

        private Vector2 FindNeighbourPoint(Direction direction, Vector2 point, Vector2 step)
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

            //result.X = result.X > _field.Bounds.Width ? step.X / 2 : result.X < 0 ? _field.Bounds.Width - step.X / 2 : result.X;
            //result.Y = result.Y > _field.Bounds.Height ? step.Y / 2 : result.Y < 0 ? _field.Bounds.Height - step.Y / 2 : result.Y;

            return result;
        }
    }
}
