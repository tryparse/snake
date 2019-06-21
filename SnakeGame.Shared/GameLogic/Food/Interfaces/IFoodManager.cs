using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
    }
}
