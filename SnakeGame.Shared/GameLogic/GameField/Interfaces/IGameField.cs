using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.GameField.Cells.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.GameField.Interfaces
{
    public interface IGameField
    {
        Rectangle Bounds { get; }

        int Columns { get; }

        int Rows { get; }

        ICell[,] Cells { get; }

        ICell GetRandomCell();
    }
}
