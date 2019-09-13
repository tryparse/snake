using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Shared.GameLogic.Snake.Interfaces
{
    public interface ISnakeGameComponent
    {
        void Reset();

        void ToggleEnabled();

        void ToggleEnabled(bool enabled);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
