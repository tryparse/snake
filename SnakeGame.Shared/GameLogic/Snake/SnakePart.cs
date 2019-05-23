using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake
{
    public class SnakePart
    {
        public SnakePart(Vector2 position, Size2 size, Direction direction)
        {
            this.Position = position;
            this.Size = size;
            this.Direction = direction;
        }

        public Vector2 Position { get; set; }
        public Size2 Size { get; set; }
        public Direction Direction { get; set; }
        public Rectangle Bounds
        {
            get
            {
                var rectangle = new Rectangle(
                    (int)(Position.X - Size.Width / 2),
                    (int)(Position.Y - Size.Height / 2),
                    (int)Size.Width,
                    (int)Size.Height
                );

                return rectangle;
            }
        }
    }
}
