using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace snake
{
    public class Cell
    {
        public Vector2 Position { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public CellType CellType { get; private set; }

        private RectangleF _bounds;

        public Cell(Vector2 position, int width, int height, CellType cellType)
        {
            Position = position;
            Width = width;
            Height = height;
            CellType = cellType;

            _bounds = new RectangleF(Position.X, Position.Y, Width, Height);
        }

        public Vector2 Center
        {
            get { return new Vector2(Position.X + Width / 2, Position.Y + Height / 2); }
        }

        public RectangleF Bounds => _bounds;
    }
}
