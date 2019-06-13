using SnakeGame.Shared.GameLogic.Food;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;

namespace SnakeGame.Shared.GameLogic
{
    // TODO: Make this manager as IUpdatable component
    public class GameManager : IGameManager
    {
        private readonly ILogger _logger;
        private readonly IFoodManager _foodManager;

        public GameManager(ILogger logger, IFoodManager foodManager)
        {
            _logger = logger;
            _foodManager = foodManager;
        }

        public void NewGame(ISnake snake)
        {
            _logger.Debug("GameManager.NewGame()");
            snake.Reset();
        }

        public bool CheckSnakeCollision(ISnake snake)
        {
            var head = snake.Segments.First();

            var tail = snake.Segments.Skip(1);

            foreach (var part in tail)
            {
                if (head.Bounds.Intersects(part.Bounds))
                {
                    _logger.Debug($"Collision: r1={head.Bounds} r2={part.Bounds}");
                    return true;
                }
            }

            return false;
        }

        public bool CheckFoodCollision(ISnake snake)
        {
            var head = snake.Segments.First.Value;

            foreach (var foodComponent in _foodManager.FoodComponents)
            {
                if (head.Bounds.Intersects(foodComponent.Food.Bounds))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<IFoodGameComponent> GetEatenFoods(ISnake snake)
        {
            if (snake == null)
            {
                throw new ArgumentNullException(nameof(snake));
            }

            var head = snake.Segments.First.Value;

            if (head == null)
            {
                throw new InvalidOperationException("Snake has no parts");
            }

            var result = new List<IFoodGameComponent>();

            foreach (var foodComponent in _foodManager.FoodComponents)
            {
                if (head.Bounds.Intersects(foodComponent.Food.Bounds))
                {
                    result.Add(foodComponent);
                }
            }

            return result;
        }

        public void RemoveFood(IEnumerable<IFoodGameComponent> foods)
        {
            foreach (var food in foods)
            {
                _foodManager.Remove(food);
            }
        }

        public void GenerateRandomFood()
        {
            var food = _foodManager.GenerateRandomFood();
            _foodManager.Add(food);
        }
    }
}
