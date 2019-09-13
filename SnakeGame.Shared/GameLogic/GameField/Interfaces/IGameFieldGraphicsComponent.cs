using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.GameField.Interfaces
{
    public interface IGameFieldGraphicsComponent
    {
        void DrawGrass(SpriteBatch spriteBatch);

        void DrawTrees(SpriteBatch spriteBatch);
    }
}
