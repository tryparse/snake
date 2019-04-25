using Microsoft.Xna.Framework;
using snake.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameEntities
{
    public class Snake
    {
        private readonly Field _field;
        private Cell _headCell;
        private IEnumerable<Vector2> _tail;

        private int stepTime;
        private int elapsedTime;

        public Snake(Field field, Cell headCell, SnakeDirection direction = SnakeDirection.Right)
        {
            this._field = field;
            this._headCell = headCell;
            this._tail = new HashSet<Vector2>();
            this.CurrentDirection = direction;
            this.stepTime = 1000;
        }

        public SnakeDirection CurrentDirection { get; set; }

        public Vector2 Position => _headCell.Bounds.Center.ToVector2();

        public void AddTail()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= stepTime)
            {
                elapsedTime = elapsedTime - stepTime;

                var stepX = _headCell.Indices.X >= _field.Width - 1 ? 0 : _headCell.Indices.X + 1;
                var stepY = _headCell.Indices.Y;

                _headCell = _field.Cells[stepX, stepY];
            }
        }
    }
}
