using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.GameLogic.Food
{
    public class FoodComponent : IFoodGameComponent
    {
        private readonly IGraphics2DComponent _graphicsComponent;

        public FoodComponent(IFoodEntity food, IGraphics2DComponent graphicsComponent, string id)
        {
            Food = food;
            ID = id;
            _graphicsComponent = graphicsComponent;
        }

        public string ID { get; }

        public IFoodEntity Food { get; }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _graphicsComponent.Draw(spriteBatch, gameTime);
        }
    }
}
