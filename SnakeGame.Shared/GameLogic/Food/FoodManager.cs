using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;

namespace SnakeGame.Shared.GameLogic.Food
{
    public class FoodManager : IFoodManager
    {
        private readonly List<IFoodGameComponent> _foods;
        private readonly Game _game;
        private readonly IGameFieldEntity _gameField;
        private readonly IGameSettings _gameSettings;
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly ILogger _logger;
        private readonly ISnakeEntity _snake;
        private int _counter;

        public FoodManager(Game game, IGameFieldEntity field, IGameSettings gameSettings, IGraphicsSystem graphicsSystem, ILogger logger, ISnakeEntity snake)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _gameField = field ?? throw new ArgumentNullException(nameof(field));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _snake = snake ?? throw new ArgumentNullException(nameof(snake));

            _foods = new List<IFoodGameComponent>();
        }

        public IEnumerable<IFoodGameComponent> FoodComponents => _foods;

        public void Add(IFoodGameComponent food)
        {
            _logger.Debug($"FoodManager.Add(): BEGIN");

            _foods.Add(food);

            _logger.Debug($"FoodManager.Add(): COMPLETE");
        }

        public void Remove(IFoodGameComponent food)
        {
            _logger.Debug($"FoodManager.Remove(): BEGIN");

            _foods.Remove(food);

            _logger.Debug($"FoodManager.Remove(): COMPLETE");
        }

        public IFoodGameComponent GenerateRandomFood()
        {
            _logger.Debug($"FoodManager.GenerateRandomFood(): BEGIN");

            var randomPosition = _gameField
                .GetRandomCellWithoutSnake(_snake)
                .Bounds
                .Center
                .ToVector2();

            var food = GenerateFood(randomPosition);

            _logger.Debug($"FoodManager.GenerateRandomFood(): COMPLETE");

            return food;
        }

        public IFoodGameComponent GenerateFood(Vector2 position)
        {
            _logger.Debug($"FoodManager.GenerateFood(): BEGIN");

            var size = new Size2(_gameSettings.TileSize, _gameSettings.TileSize);

            var food = new FoodEntity(position, size);
            var graphicsComponent = new FoodGraphicsComponent(food, _graphicsSystem);

            var component = new FoodComponent(food, graphicsComponent, _counter.ToString());

            _counter++;

            _logger.Debug($"FoodManager.GenerateFood(): COMPLETE");

            return component;
        }

        public void Reset()
        {
            foreach (var c in FoodComponents)
            {
                //_game.Components.Remove(c);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var foodGameComponent in _foods)
            {
                foodGameComponent.Draw(spriteBatch);
            }
        }
    }
}
