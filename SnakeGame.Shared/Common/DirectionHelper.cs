using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SnakeGame.Shared.Common
{
    public static class DirectionHelper
    {
        private static readonly Random Random = new Random();

        private static readonly Dictionary<Direction, Direction> Opposites = new Dictionary<Direction, Direction>
        {
            { Direction.Down, Direction.Up },
            { Direction.Up, Direction.Down },
            { Direction.Left, Direction.Right },
            { Direction.Right, Direction.Left }
        };

        private static readonly Dictionary<Direction, float> Rotations = new Dictionary<Direction, float>
        {
            { Direction.Up, MathHelper.ToRadians(-90) },
            { Direction.Right, MathHelper.ToRadians(0) },
            { Direction.Down, MathHelper.ToRadians(90) },
            { Direction.Left, MathHelper.ToRadians(180) }
        };

        public static Direction GetOppositeDirection(Direction direction)
        {
            return Opposites[direction];
        }

        public static Direction GetRandom()
        {
            var dice = Random.Next(0, 4);

            var values = (Direction[])Enum.GetValues(typeof(Direction));

            return values[dice];
        }

        public static float GetRotation(Direction direction)
        {
            return Rotations[direction];
        }

        public static Vector2 RotateVector(Vector2 input, Direction toDirection)
        {
            var rotation = GetRotation(toDirection);
            var rotationMatrix = Matrix.CreateRotationZ(rotation);

            return Vector2.Transform(input, rotationMatrix);
        }
    }
}
