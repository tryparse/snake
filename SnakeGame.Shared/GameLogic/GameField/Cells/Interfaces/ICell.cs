using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.GameField.Cells.Interfaces
{
    public interface ICell
    {
        Vector2 Position { get; }

        int Width { get; }

        int Height { get; }

        CellType CellType { get; }

        int Column { get; }

        int Row { get; }

        Rectangle Bounds { get; }
    }
}
