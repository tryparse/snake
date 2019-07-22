using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake
{
    class SnakeSegment : ISnakeSegment
    {
        public SnakeSegment(Vector2 position, Size2 size, Direction direction)
        {
            Position = position;
            Size = size;
            Direction = direction;

            RecalculateBounds();
        }

        public Vector2 Position { get; private set; }

        public Size2 Size { get; }

        public Direction Direction { get; private set; }

        public Rectangle Bounds { get; private set; }

        private void RecalculateBounds()
        {
            Bounds = new Rectangle(
                    (int)(Position.X - Size.Width / 2),
                    (int)(Position.Y - Size.Height / 2),
                    (int)Size.Width,
                    (int)Size.Height
                );
        }

        public void MoveTo(Vector2 position)
        {
            Position = position;

            RecalculateBounds();
        }

        public void SetDirection(Direction direction)
        {
            Direction = direction;
        }
    }
}
