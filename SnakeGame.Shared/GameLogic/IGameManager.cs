using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.Snake;
using System.Collections.Generic;

namespace SnakeGame.Shared.GameLogic
{
    public interface IGameManager
    {
        void NewGame(SnakeComponent snakeComponent);

        bool CheckSnakeCollision(SnakeComponent snakeComponent);

        bool CheckFoodCollision(SnakeComponent snake);

        IEnumerable<IFoodGameComponent> GetEatenFoods(SnakeComponent snake);

        void RemoveFood(IEnumerable<IFoodGameComponent> foods);

        void GenerateRandomFood();
    }
}
