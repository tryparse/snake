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
        private Vector2 _position;
        private Size2 _size;
        private Direction _direction;
        private Rectangle _bounds;

        public SnakeSegment(Vector2 position, Size2 size, Direction direction)
        {
            _position = position;
            _size = size;
            _direction = direction;

            RecalculateBounds();
        }

        public Vector2 Position => _position;

        public Size2 Size => _size;

        public Direction Direction => _direction;

        public Rectangle Bounds => _bounds;

        private void RecalculateBounds()
        {
            _bounds = new Rectangle(
                    (int)(Position.X - Size.Width / 2),
                    (int)(Position.Y - Size.Height / 2),
                    (int)Size.Width,
                    (int)Size.Height
                );
        }

        public void MoveTo(Vector2 position)
        {
            _position = position;

            RecalculateBounds();
        }

        public void SetDirection(Direction direction)
        {
            _direction = direction;
        }
    }
}
