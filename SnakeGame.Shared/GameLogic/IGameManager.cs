using SnakeGame.Shared.GameLogic.Snake;

namespace SnakeGame.Shared.GameLogic
{
    public interface IGameManager
    {
        void NewGame(SnakeComponent snakeComponent);

        bool CheckSnakeCollision(SnakeComponent snakeComponent);

        bool CheckFoodEating(SnakeComponent snake);
    }
}
