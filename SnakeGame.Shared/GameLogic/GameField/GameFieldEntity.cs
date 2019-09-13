using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField.Cells.Interfaces;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;

namespace SnakeGame.Shared.GameLogic.GameField
{
    public class GameFieldEntity : IGameFieldEntity
    {
        private readonly IRandomGenerator _random;

        public GameFieldEntity(ICell[,] cells, IGameSettings gameSettings, IRandomGenerator random)
        {
            Cells = cells;

            Bounds = new Rectangle(0, 0, Columns * gameSettings.TileSize, Rows * gameSettings.TileSize);

            _random = random;
        }

        public Rectangle Bounds { get; }

        public int Columns => Cells.GetLength(0);

        public int Rows => Cells.GetLength(1);

        public ICell[,] Cells { get; }

        public ICell GetRandomCell()
        {
            var x = _random.Next(0, Columns);
            var y = _random.Next(0, Rows);

            return Cells[x, y];
        }

        public ICell GetRandomCellWithoutSnake(ISnakeEntity snake)
        {
            var segments = new List<ISnakeSegment>(snake.Tail) {snake.Head};

            var snakeCells = segments.Select(x => GetCellByCoordinates(x.Position))
                .ToList();

            var cells = (from ICell c in Cells where !snakeCells.Contains(c) select c).ToList();

            var index = _random.Next(0, cells.Count);

            return cells[index];
        }

        public ICell GetCellByCoordinates(Vector2 position)
        {
            if (!Bounds.Contains(position))
            {
                return null;
            }

            var x = (int) Math.Floor((position.X / Bounds.Width) * Columns);
            var y = (int) Math.Floor((position.Y / Bounds.Height) * Rows);

            return Cells[x, y];
        }
    }
}
