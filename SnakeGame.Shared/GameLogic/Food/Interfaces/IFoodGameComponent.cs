﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.GameLogic.Food.Interfaces
{
    public interface IFoodGameComponent
    {
        string ID { get; }

        Rectangle Bounds { get; }

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);

        void DrawDebug(SpriteBatch spriteBatch);
    }
}
