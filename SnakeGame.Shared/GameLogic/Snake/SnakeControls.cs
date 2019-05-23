using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class SnakeControls
    {
        public SnakeControls(Keys up, Keys down, Keys left, Keys right)
        {
            this.Up = up;
            this.Down = down;
            this.Left = left;
            this.Right = right;
        }

        public Keys Up { get; }

        public Keys Down { get; }

        public Keys Left { get; }

        public Keys Right { get; }
    }
}

