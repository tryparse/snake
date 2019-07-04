using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.Snake;
using System.Collections.Generic;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using Microsoft.Xna.Framework;

namespace SnakeGame.Shared.GameLogic
{
    public interface IGameManager : IGameComponent, IUpdateable
    {
        void NewGame();

        bool CheckSnakeCollision();

        bool CheckFoodCollision();

        bool CheckWallsCollision();

        IEnumerable<IFoodGameComponent> GetEatenFoods();

        void RemoveFood(IEnumerable<IFoodGameComponent> foods);

        void GenerateRandomFood();

        void IncreaseGameSpeed();
    }
}
