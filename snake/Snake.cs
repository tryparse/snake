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
        private Vector2 _headPosition;
        private IEnumerable<Vector2> _tail;
        private int _length;

        public Snake(Vector2 headPosition, SnakeDirection direction = SnakeDirection.Right)
        {
            _headPosition = headPosition;
            _tail = new HashSet<Vector2>();
            _length = 1;
            CurrentDirection = direction;
        }

        public SnakeDirection CurrentDirection { get; set; }

        public Vector2 Position => _headPosition;

        public void AddTail()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }
    }
}
