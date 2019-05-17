using Microsoft.Xna.Framework;
using snake.Common;
using SnakeGame.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameEntities
{
    public class FieldFactory : IFieldFactory
    {
        private readonly IGameSettings _settings;

        public FieldFactory(IGameSettings settings)
        {
            _settings = settings;
        }

        public Field GetRandomField(int width, int height, double grassProbability = .8d)
        {
            var random = new Random();
            var cells = new Cell[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var fieldTypeDice = random.NextDouble();
                    var cellType = fieldTypeDice <= grassProbability ? CellType.Grass : CellType.Tree;

                    cells[x, y] = new Cell(
                        position: new Vector2(x * _settings.TileWidth, y * _settings.TileHeight),
                        indices: new Point(x, y),
                        width: _settings.TileWidth,
                        height: _settings.TileHeight,
                        cellType: cellType );
                }
            }

            var field = new Field(_settings, width, height, cells);

            return field;
        }
    }
}
