using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.GameLogic.GameField.Cells.Interfaces;

namespace SnakeGame.Shared.GameLogic.GameField.Cells
{
    public class Cell : ICell
    {
        public Cell(Vector2 position, int column, int row, int width, int height, CellType cellType)
        {
            Position = position;
            Column = column;
            Row = row;
            Width = width;
            Height = height;
            CellType = cellType;
            RecalculateBounds();
        }

        public Vector2 Position { get; }

        public int Width { get; }

        public int Height { get; }

        public CellType CellType { get; }

        public int Column { get; }

        public int Row { get; }

        public Rectangle Bounds { get; private set; }

        public BoundingRectangle BoundingRectangle { get; private set; }

        private void RecalculateBounds()
        {
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            BoundingRectangle = new BoundingRectangle(Bounds.Center, new Size2((float)Width / 2f, (float)Height / 2f));
        }
    }
}
