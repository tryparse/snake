﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameEntities.Fruit
{
    class Fruit
    {
        public Fruit(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public Vector2 Position { get; }
        public Vector2 Size { get; }
    }
}
