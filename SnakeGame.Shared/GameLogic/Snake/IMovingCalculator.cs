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
        Vector2 Calculate(Vector2 currentPosition, Vector2 targetPosition, TimeSpan transitionTime, TimeSpan elapsedTransitionTime);

        Vector2 FindNeighbourPoint(Direction direction, Vector2 point, Vector2 step);

        Vector2 CheckBoundaries(Vector2 step, Vector2 result);
    }
}
