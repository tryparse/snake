using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Fruit
{
    public class Fruit
    {
        public Fruit(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public Vector2 Position { get; }
        public Size2 Size { get; }

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
