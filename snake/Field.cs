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
		public int Width { get; private set; }
		public int Height { get; private set; }

        public Cell[,] Cells { get; }

        public Field(int width, int height, Cell[,] cells)
		{
			Width = width;
			Height = height;
            Cells = cells;
        }

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
