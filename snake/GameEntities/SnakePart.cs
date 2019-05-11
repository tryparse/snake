﻿using Microsoft.Xna.Framework;
using snake.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameEntities
{
    public class SnakePart
    {
        public SnakePart(Vector2 position, Vector2 size, Direction direction)
        {
            this.Position = position;
            this.Size = size;
            this.Direction = direction;
        }

        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Direction Direction { get; set; }
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)(Position.X - Size.X / 2),
                    (int)(Position.Y - Size.Y / 2),
                    (int)Size.X, 
                    (int)Size.Y
                );
            }
        }
    }
}
