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
        private readonly IFoodEntity _food;

        public FoodComponent(IFoodEntity food, IGraphics2DComponent graphicsComponent, string id)
        {
            _food = food;
            ID = id;
            _graphicsComponent = graphicsComponent;
        }

        public string ID { get; }

        public Rectangle Bounds => _food.Bounds;

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphicsComponent.Draw(spriteBatch);
        }

        public void DrawDebug(SpriteBatch spriteBatch)
        {
            _graphicsComponent.DebugDraw(spriteBatch);
        }
    }
}
