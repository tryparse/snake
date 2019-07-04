using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public interface IMovingCalculator
    {
        Vector2 CalculateMoving(Vector2 currentPosition, Vector2 targetPosition, TimeSpan transitionTime, TimeSpan elapsedTransitionTime);

        Vector2 CalculateTargetPosition(Direction direction, Vector2 position, Vector2 step);
    }
}
