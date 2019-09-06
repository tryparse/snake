using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Shared.UIComponents
{
    internal interface IUiComponent
    {
        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}