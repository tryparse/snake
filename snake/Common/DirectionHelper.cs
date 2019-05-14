using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.Common
{
    static class DirectionHelper
    {
        private static Dictionary<Direction, Direction> _opposites = new Dictionary<Direction, Direction>
        {
            { Direction.Down, Direction.Up },
            { Direction.Up, Direction.Down },
            { Direction.Left, Direction.Right },
            { Direction.Right, Direction.Left }
        };

        public static Direction GetOppositeDirection(Direction direction)
        {
            return _opposites[direction];
        }

        public static Direction GetRandom()
        {
            var random = new Random();

            var dice = random.Next(0, 4);

            var values = (Direction[])Enum.GetValues(typeof(Direction));

            return values[dice];
        }
    }
}
