using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.GameField.Cells.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using MonoGame.Extended;

namespace SnakeGame.Shared.GameLogic.GameField.Interfaces
{
    public interface IGameFieldEntity
    {
        Rectangle Bounds { get; }

        int Columns { get; }

        int Rows { get; }

        ICell[,] Cells { get; }

        IEnumerable<ICell> GetCells();

        ICell GetRandomCell();

        ICell GetRandomCellWithoutSnake(ISnakeEntity snake);

        ICell GetCellByCoordinates(Vector2 position);

        IEnumerable<ICell> GetVisibleCells(Vector2 pov, float radius);

        IEnumerable<Ray2D> GetRays(Vector2 pov, float radius);
    }
}
