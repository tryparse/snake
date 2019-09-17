using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Shared.GameLogic.Food.Interfaces
{
    public interface IFoodManager
    {
        IEnumerable<IFoodGameComponent> FoodComponents { get; }

        /// <summary>
        /// Return a random generated food
        /// </summary>
        /// <returns></returns>
        IFoodGameComponent GenerateRandomFood();

        IFoodGameComponent GenerateFood(Vector2 position);

        void Add(IFoodGameComponent food);

        void Remove(IFoodGameComponent food);

        void Reset();

        void Draw(SpriteBatch spriteBatch);

        void DebugDraw(SpriteBatch spriteBatch);
    }
}
