using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace SnakeGame.Shared.GameLogic.Food.Interfaces
{
    public interface IFoodEntity
    {
        Vector2 Position { get; }

        Size2 Size { get; }

        Rectangle Bounds { get; }

        float Rotation { get; }
    }
}
