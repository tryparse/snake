using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.Common.ResourceManagers;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.Graphics
{
    public interface IGraphics2DComponent
    {
        void Draw(SpriteBatch spriteBatch);

        void DebugDraw(SpriteBatch spriteBatch);
    }
}
