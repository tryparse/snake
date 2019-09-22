using SnakeGame.Shared.GameLogic.Food.Interfaces;
using Microsoft.Xna.Framework;

namespace SnakeGame.Shared.GameLogic
{
    public interface IGameManager : IGameComponent, IUpdateable
    {
        void NewGame();

        bool CheckSnakeCollision();

        IFoodGameComponent CheckFoodCollision();

        bool CheckWallsCollision();

        void RemoveFood(IFoodGameComponent food);

        void GenerateRandomFood();

        void IncreaseGameSpeed();

        bool IsPaused { get; }

        void TogglePause();

        void TogglePause(bool isPaused);
    }
}
