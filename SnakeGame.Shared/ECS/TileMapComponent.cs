using System.Collections.Generic;
using MonoGame.Extended;

namespace SnakeGame.Shared.ECS
{
    class TileMapComponent
    {
        private TileCell[,] _grid;
        private Size _size;

        public TileMapComponent(Size size)
        {
            _size = size;
            _grid = new TileCell[size.Width, size.Height];
        }

        public TileCell[,] Grid => _grid;

        public IEnumerable<TileCell> GetCells()
        {
            var cells = new List<TileCell>();

            foreach (var cell in _grid)
            {
                cells.Add(cell);
            }

            return cells;
        }
    }
}
