using Microsoft.Xna.Framework;
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
        void DrawGrass(SpriteBatch spriteBatch, Vector2 pointOfView, float viewRadius);

        void DrawTrees(SpriteBatch spriteBatch);

        void DrawGrassDebug(SpriteBatch spriteBatch);

        void DrawTreesDebug(SpriteBatch spriteBatch);

        void DrawBorders(SpriteBatch spriteBatch);
    }
}
