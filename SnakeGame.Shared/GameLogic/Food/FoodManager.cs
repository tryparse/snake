using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.GameField;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Renderers;
using SnakeGame.Shared.Settings;
using System;
using System.Collections.Generic;

namespace SnakeGame.Shared.GameLogic.Food
{
    public class FoodManager : IFoodManager
    {
        private readonly List<IFoodGameComponent> _foods;
        private readonly Game _game;
        private readonly IGameField _gameField;
        private readonly IGameSettings _gameSettings;
        private readonly IRenderingSystem _renderingCore;
        private readonly Random _random;

        public FoodManager(Game game, IGameField field, IGameSettings gameSettings, IRenderingSystem renderingCore)
        {
            _foods = new List<IFoodGameComponent>();
            _game = game;
            _gameField = field;
            _gameSettings = gameSettings;
            _renderingCore = renderingCore;
            _random = new Random();
        }

        public IEnumerable<IFoodGameComponent> FoodComponents => _foods;

        public void Add(IFoodGameComponent food)
        {
            _foods.Add(food);
            _game.Components.Add(food);
        }

        public void Remove(IFoodGameComponent food)
        {
            _foods.Remove(food);
            _game.Components.Remove(food);
        }

        public IFoodGameComponent GenerateRandomFood()
        {
            var randomPosition = _gameField.Cells[_random.Next(0, _gameField.Columns), _random.Next(0, _gameField.Rows)].Bounds.Center.ToVector2();

            return GenerateFood(randomPosition);
        }

        public IFoodGameComponent GenerateFood(Vector2 position)
        {
            var size = new Size2(_gameSettings.TileWidth, _gameSettings.TileHeight);

            var food = new Food(position, size);
            var graphicsComponent = new FoodGraphicsComponent(food, _renderingCore);

            var component = new FoodComponent(food, graphicsComponent)
            {
                Enabled = true,
                Visible = true
            };

            return component;
        }
    }
}
