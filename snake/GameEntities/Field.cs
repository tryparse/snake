using Microsoft.Xna.Framework;
using snake.Common;
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

        public Field(int width, int height, Cell[,] cells)
		{
            this._cells = cells;
        }

        public Cell[,] Cells => _cells;
        public int Width => _cells.GetLength(0);
        public int Height => _cells.GetLength(1);

        //public Cell GetNeighbour(Cell forCell, Common.Direction direction)
        //{
        //    var fieldWidth = Cells.GetLength(0);
        //    var fieldHeight = Cells.GetLength(1);

        //    switch (direction)
        //    {
        //        case Direction.Down:
        //            {
        //                Cells.GetL

        //                break;
        //            }
        //    }
        //}
    }
}
