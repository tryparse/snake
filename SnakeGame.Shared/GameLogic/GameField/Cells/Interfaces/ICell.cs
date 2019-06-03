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
        int Column { get; }
        int Row { get; }
        CellType CellType { get; }
        Rectangle Bounds { get; }
    }
}
