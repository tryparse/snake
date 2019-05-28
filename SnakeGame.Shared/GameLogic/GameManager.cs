using SnakeGame.Shared.GameLogic.Food;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic
{
    public class GameManager : IGameManager
    {
        private readonly ILogger _logger;
        private readonly IFoodManager _foodManager;

        public GameManager(ILogger logger, IFoodManager foodManager)
        {
            _logger = logger;
            _foodManager = foodManager;
        }

        public void NewGame(SnakeComponent snakeComponent)
        {
            _logger.Debug("GameManager.NewGame()");
            snakeComponent.Reset();
        }

        public bool CheckSnakeCollision(SnakeComponent snakeComponent)
        {
            var head = snakeComponent.Parts.First();

            var tail = snakeComponent.Parts.Skip(1);

            foreach (var part in tail)
            {
                if (head.Bounds.Intersects(part.Bounds))
                {
                    return true;
                }
            }

            return false;
        }

#warning SPR violation
        public bool CheckFoodEating(SnakeComponent snake)
        {
            var head = snake.Parts.First.Value;

            foreach (var foodComponent in _foodManager.FoodComponents)
            {
                if (head.Bounds.Intersects(foodComponent.Food.Bounds))
                {
                    _foodManager.Remove(foodComponent);
                    snake.AddTail();
                    // TODO: adding food;

                    return true;
                }
            }

            return false;
        }
    }
}
