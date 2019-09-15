using Microsoft.Xna.Framework;
using SnakeGame.Shared.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Shared.GameLogic.GameField.Interfaces
{
    public interface IGameFieldComponent
    {
        void Update(GameTime gameTime);

        void DrawGrass(SpriteBatch spriteBatch);

        void DrawGrassDebug(SpriteBatch spriteBatch);

        void DrawTrees(SpriteBatch spriteBatch);

        void DrawTreesDebug(SpriteBatch spriteBatch);
    }
}
