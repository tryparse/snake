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
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;

namespace SnakeGame.Shared.GameLogic
{
    // TODO: Make this manager as IUpdatable component
    public class GameManager : IGameManager
    {
        private readonly ILogger _logger;
        private readonly IFoodManager _foodManager;
        private readonly ISnake _snake;
        private readonly IGameField _gameField;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public GameManager(ILogger logger, IFoodManager foodManager, ISnake snake, IGameField gameField)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _foodManager = foodManager ?? throw new ArgumentNullException(nameof(foodManager));
            _snake = snake ?? throw new ArgumentNullException(nameof(snake));
            _gameField = gameField ?? throw new ArgumentNullException(nameof(gameField));

            Initialize();
        }

        public void NewGame()
        {
            _logger.Debug("GameManager.NewGame()");
            _snake.Reset();
        }

        public bool CheckSnakeCollision()
        {
            var head = _snake.Segments.First();

            var tail = _snake.Segments.Skip(1);

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

        public bool CheckFoodCollision()
        {
            var head = _snake.Segments.First.Value;

            foreach (var foodComponent in _foodManager.FoodComponents)
            {
                if (head.Bounds.Intersects(foodComponent.Food.Bounds))
                {
                    _logger.Debug("GameManager: Detected food collision");
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<IFoodGameComponent> GetEatenFoods()
        {
            var head = _snake.Segments.First.Value;

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

            _logger.Debug("GameManager: Removed food");
        }

        public void GenerateRandomFood()
        {
            var food = _foodManager.GenerateRandomFood();
            _foodManager.Add(food);

            _logger.Debug("GameManager: Random food generated");
        }

        public bool CheckWallsCollision()
        {
            var head = _snake.Segments
                .FirstOrDefault();

            if (head != null
                && (head.Bounds.Left < _gameField.Bounds.Left
                    || head.Bounds.Right > _gameField.Bounds.Right
                    || head.Bounds.Top < _gameField.Bounds.Top
                    || head.Bounds.Bottom > _gameField.Bounds.Bottom))
            {
                _logger.Debug("GameManager: Detected walls collision");
                return true;
            }

            return false;
        }

        public void Initialize()
        {
            Enabled = true;
        }

        public void Update(GameTime gameTime)
        {
            if (CheckSnakeCollision() || CheckWallsCollision())
            {
                NewGame();
            }

            if (CheckFoodCollision())
            {
                RemoveFood(GetEatenFoods());

                _snake.AddSegments(1);
                _logger.Debug($"GameManager.Update({gameTime}): AddedSegment");
            }
        }
    }
}
