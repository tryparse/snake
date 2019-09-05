using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.GameLogic.Food.Interfaces;

namespace SnakeGame.Shared.GameLogic.Food
{
    public class FoodEntity : IFoodEntity
    {
        public FoodEntity(Vector2 position, Size2 size, float rotation = 0f)
        {
            Position = position;
            Size = size;
            Rotation = rotation;

            RecalcBoundsRectangle();
        }

        public Vector2 Position { get; }

        public Size2 Size { get; }

        public Rectangle Bounds { get; private set; }

        public float Rotation { get; }

        private void RecalcBoundsRectangle()
        {
            Bounds = new Rectangle(
                (int)(Position.X - Size.Width / 2),
                (int)(Position.Y - Size.Height / 2),
                (int)Size.Width,
                (int)Size.Height
            );
        }
    }
}
