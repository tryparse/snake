using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common.ResourceManagers;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.Graphics
{
    public interface IGraphics2DComponent
    {
        void Draw(GameTime gameTime);
    }
}
