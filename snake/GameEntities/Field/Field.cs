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
	public class Field
	{
        private readonly Cell[,] _cells;
        private readonly Rectangle _bounds;

        public Field(IGameSettings settings, int width, int height, Cell[,] cells)
		{
            this._cells = cells;
            this._bounds = new Rectangle(0, 0, LengthX * settings.TileWidth, LengthY * settings.TileHeight);
        }

        public Cell[,] Cells => _cells;

        public int LengthX => _cells.GetLength(0);

        public int LengthY => _cells.GetLength(1);

        public Rectangle Bounds => _bounds;
    }
}
