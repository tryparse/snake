using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Graphics;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Shared.GameLogic.GameField
{
    public class GameFieldComponent : IGameFieldComponent
    {
        private readonly IGraphics2DComponent _graphicsComponent;

        public GameFieldComponent(IGameField gameField, IGraphics2DComponent graphicsComponent)
        {
            _graphicsComponent = graphicsComponent;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _graphicsComponent.Draw(spriteBatch, gameTime);
        }
    }
}
