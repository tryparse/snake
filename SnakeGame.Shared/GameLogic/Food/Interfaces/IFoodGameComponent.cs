using Microsoft.Xna.Framework;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.GameLogic.Food.Interfaces
{
    public interface IFoodGameComponent : IGameComponent, IUpdateable, IDrawable
    {
        string ID { get; }

        IFoodEntity Food { get; }
    }
}
