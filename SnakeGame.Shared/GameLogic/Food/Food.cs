using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.GameLogic.Food.Interfaces;

namespace SnakeGame.Shared.GameLogic.Food
{
    public class Food : IFood
    {
        private Vector2 _position;
        private Size2 _size;
        private float _rotation;
        private Rectangle _bounds;

        public Food(Vector2 position, Size2 size, float rotation = 0f)
        {
            _position = position;
            _size = size;
            _rotation = rotation;

            RecalcBoundsRectangle();
        }

        public Vector2 Position => _position;

        public Size2 Size => _size;

        public Rectangle Bounds => _bounds;

        public float Rotation => _rotation;

        private void RecalcBoundsRectangle()
        {
            _bounds = new Rectangle(
                                (int)(Position.X - Size.Width / 2),
                                (int)(Position.Y - Size.Height / 2),
                                (int)Size.Width,
                                (int)Size.Height
                            );
        }
    }
}
