using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SnakeGame.Shared.Common
{
    public static class DirectionHelper
    {
        private static Random _random = new Random();

        private static Dictionary<Direction, Direction> _opposites = new Dictionary<Direction, Direction>
        {
            { Direction.Down, Direction.Up },
            { Direction.Up, Direction.Down },
            { Direction.Left, Direction.Right },
            { Direction.Right, Direction.Left }
        };

        private static Dictionary<Direction, float> _rotations = new Dictionary<Direction, float>
            {
                { Direction.Up, MathHelper.ToRadians(-90) },
                { Direction.Right, MathHelper.ToRadians(0) },
                { Direction.Down, MathHelper.ToRadians(90) },
                { Direction.Left, MathHelper.ToRadians(180) }
            };

        public static Direction GetOppositeDirection(Direction direction)
        {
            return _opposites[direction];
        }

        public static Direction GetRandom()
        {
            var dice = _random.Next(0, 4);

            var values = (Direction[])Enum.GetValues(typeof(Direction));

            return values[dice];
        }

        public static float GetRotation(Direction direction)
        {
            return _rotations[direction];
        }
    }
}
