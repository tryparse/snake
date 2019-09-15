using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Graphics;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Shared.GameLogic.GameField
{
    public class GameFieldComponent : IGameFieldComponent
    {
        private readonly IGameFieldGraphicsComponent _gameFieldGraphicsComponent;

        public GameFieldComponent(IGameFieldEntity gameField, IGameFieldGraphicsComponent gameFieldGraphicsComponent)
        {
            _gameFieldGraphicsComponent = gameFieldGraphicsComponent;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void DrawGrass(SpriteBatch spriteBatch)
        {
            _gameFieldGraphicsComponent.DrawGrass(spriteBatch);
        }

        public void DrawGrassDebug(SpriteBatch spriteBatch)
        {
            _gameFieldGraphicsComponent.DrawGrassDebug(spriteBatch);
        }

        public void DrawTrees(SpriteBatch spriteBatch)
        {
            _gameFieldGraphicsComponent.DrawTrees(spriteBatch);
        }

        public void DrawTreesDebug(SpriteBatch spriteBatch)
        {
            _gameFieldGraphicsComponent.DrawTreesDebug(spriteBatch);
        }
    }
}
