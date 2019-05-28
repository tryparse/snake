using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.GameField;
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
        private readonly Field _gameField;
        private readonly IGameSettings _gameSettings;
        private readonly IRenderingCore _renderingCore;
        private readonly Random _random;

        public FoodManager(Game game, Field field, IGameSettings gameSettings, IRenderingCore renderingCore)
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
            var randomPosition = _gameField.Cells[_random.Next(0, _gameField.ColumnsCount), _random.Next(0, _gameField.RowsCount)].Bounds.Center.ToVector2();

            return GenerateFood(randomPosition);
        }

        public IFoodGameComponent GenerateFood(Vector2 position)
        {
            var size = new Size2(_gameSettings.TileWidth, _gameSettings.TileHeight);

            var food = new Food(position, size);
            var graphicsComponent = new FoodGraphicsComponent(_renderingCore.SpriteBatch, _renderingCore.SpriteFont, food, _renderingCore.RenderSettings, _renderingCore.TextureManager);

            var component = new FoodComponent(food, graphicsComponent)
            {
                Enabled = true,
                Visible = true
            };

            return component;
        }
    }
}
