﻿using snake.GameEntities;

namespace snake
{
    public interface IFieldFactory
    {
        Field GetRandomField(int width, int height);
    }
}