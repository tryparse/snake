using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameEntities
{
    public class SnakeControls
    {
        public SnakeControls(Keys up, Keys down, Keys left, Keys right)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }

        public Keys Up { get; }

        public Keys Down { get; }

        public Keys Left { get; }

        public Keys Right { get; }
    }
}
