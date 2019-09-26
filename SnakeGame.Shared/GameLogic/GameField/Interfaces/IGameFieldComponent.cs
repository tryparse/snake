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

        void DrawBorders(SpriteBatch spriteBatch);

        void DrawGrass(SpriteBatch spriteBatch, Vector2 pointOfView, float viewRadius);

        void DrawTrees(SpriteBatch spriteBatch, Vector2 pointOfView, float viewRadius);

        void DrawGrassDebug(SpriteBatch spriteBatch);

        void DrawTreesDebug(SpriteBatch spriteBatch);
    }
}
