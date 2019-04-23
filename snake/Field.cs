using Microsoft.Xna.Framework;
using snake.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake
{
	public class Field
	{
        public Cell[,] Cells { get; }

        public Field(int width, int height, Cell[,] cells)
		{
            Cells = cells;
        }

        public int Width => Cells.GetLength(0);
        public int Height => Cells.GetLength(1);

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
