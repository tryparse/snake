using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.Snake;
using System.Collections.Generic;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;

namespace SnakeGame.Shared.GameLogic
{
    public interface IGameManager
    {
        void NewGame(ISnake snake);

        bool CheckSnakeCollision(ISnake snake);

        bool CheckFoodCollision(ISnake snake);

        IEnumerable<IFoodGameComponent> GetEatenFoods(ISnake snake);

        void RemoveFood(IEnumerable<IFoodGameComponent> foods);

        void GenerateRandomFood();
    }
}
