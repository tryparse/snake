using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.GameField;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Renderers;
using SnakeGame.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Fruit
{
    public class FruitManager : IFruitManager
    {
        private readonly ILogger _logger;
        private readonly IGameSettings _gameSettings;
        private readonly Field _gameField;
        private readonly GameComponentCollection _gameComponents;
        private readonly IFruitRendererFactory _fruitRendererFactory;

        private readonly Random _random;

        private readonly List<Fruit> _fruits;
        private readonly List<IRenderer2D> _renderers;

        public FruitManager(ILogger logger, IGameSettings gameSettings, GameComponentCollection gameComponents, Field gameField, IFruitRendererFactory fruitRendererFactory)
        {
            _gameComponents = gameComponents;
            _logger = logger;
            this._gameSettings = gameSettings;
            _gameField = gameField;
            _fruitRendererFactory = fruitRendererFactory;

            _random = new Random();
            _fruits = new List<Fruit>();
            _renderers = new List<IRenderer2D>();
        }

        public IEnumerable<Fruit> Fruits => _fruits;

        public void CreateFruit()
        {
            var position = _gameField.Cells[_random.Next(0, _gameField.ColumnsCount), _random.Next(0, _gameField.RowsCount)].Bounds.Center.ToVector2();

            var size = new Vector2(_gameSettings.TileWidth, _gameSettings.TileHeight);

            var fruit = new Fruit(position, size);
            var renderer = _fruitRendererFactory.GetFruitRenderer(fruit);

            _fruits.Add(fruit);
            _renderers.Add(renderer);
            _gameComponents.Add(renderer);
        }

        public void RemoveFruit(Fruit fruit)
        {
            _fruits.Remove(fruit);
        }
    }
}
