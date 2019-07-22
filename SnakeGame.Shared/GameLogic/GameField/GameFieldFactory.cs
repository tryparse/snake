using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField.Cells;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Settings;

namespace SnakeGame.Shared.GameLogic.GameField
{
    public class GameFieldFactory : IGameFieldFactory
    {
        private readonly IGameSettings _gameSettings;
        private readonly IRandomGenerator _random;

        public GameFieldFactory(IGameSettings gameSettings, IRandomGenerator random)
        {
            _gameSettings = gameSettings;

            _random = random;
        }

        public IGameField GetRandomField(int width, int height, double grassProbability)
        {
            var cells = new Cell[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var fieldTypeDice = _random.NextDouble();
                    var cellType = fieldTypeDice <= grassProbability ? CellType.Grass : CellType.Tree;

                    cells[x, y] = new Cell(
                        position: new Vector2(x * _gameSettings.TileSize, y * _gameSettings.TileSize),
                        column: x,
                        row: y,
                        width: _gameSettings.TileSize,
                        height: _gameSettings.TileSize,
                        cellType: cellType);
                }
            }

            var field = new GameField(cells, _gameSettings, _random);

            return field;
        }
    }
}
