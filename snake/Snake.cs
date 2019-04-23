using Microsoft.Xna.Framework;
using snake.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake
{
    public class Snake
    {
        private readonly Field _field;
        private Cell _headCell;
        private IEnumerable<Vector2> _tail;
        private int _length;

        private int stepTime;
        private int elapsedTime;

        public Snake(Field field, Cell headCell, SnakeDirection direction = SnakeDirection.Right)
        {
            _field = field;
            _headCell = headCell;
            _tail = new HashSet<Vector2>();
            _length = 1;
            CurrentDirection = direction;
            stepTime = 1000;
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
