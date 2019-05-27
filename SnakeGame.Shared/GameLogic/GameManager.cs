using SnakeGame.Shared.GameLogic.Fruit;
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
        private readonly IFruitManager _fruitManager;

        public GameManager(ILogger logger, IFruitManager fruitManager)
        {
            _logger = logger;
            _fruitManager = fruitManager;
        }

        public void NewGame(SnakeComponent snakeComponent)
        {
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
        public bool CheckFruitEating(SnakeComponent snake)
        {
            var head = snake.Parts.First.Value;

            foreach (var fruit in _fruitManager.Fruits)
            {
                if (head.Bounds.Intersects(fruit.Bounds))
                {
                    _fruitManager.RemoveFruit(fruit);
                    snake.AddTail();
                    _fruitManager.CreateFruit();

                    return true;
                }
            }

            return false;
        }
    }
}
