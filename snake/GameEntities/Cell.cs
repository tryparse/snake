using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace snake.GameEntities
{
    public class Cell
    {
        public Cell(Vector2 position, Point indices, int width, int height, CellType cellType)
        {
            this.Position = position;
            this.Indices = indices;
            this.Width = width;
            this.Height = height;
            this.CellType = cellType;

            this.BoundsF = new RectangleF(Position.X, Position.Y, Width, Height);
        }

        /// <summary>
        /// Cell coordinates
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Cell width
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Cell height
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Cell type
        /// </summary>
        public CellType CellType { get; }

        /// <summary>
        /// Indices in cells array
        /// </summary>
        public Point Indices { get; }

        public RectangleF BoundsF { get; }

        public Rectangle Bounds => BoundsF.ToRectangle();
    }
}
