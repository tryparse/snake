using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class SnakeControlKeys
    {
        public SnakeControlKeys(Keys up, Keys down, Keys left, Keys right)
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

