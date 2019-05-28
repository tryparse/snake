using Microsoft.Xna.Framework;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.GameLogic.Food.Interfaces
{
    public interface IFoodGameComponent : IGameComponent, IUpdateable, IDrawable
    {
        IGraphics2DComponent GraphicsComponent { get; }

        IFood Food { get; }
    }
}
