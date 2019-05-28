using System;
using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.GameLogic.Food
{
    public class FoodComponent : IFoodGameComponent
    {
        public FoodComponent(IFood food, IGraphics2DComponent graphicsComponent)
        {
            Food = food;
            GraphicsComponent = graphicsComponent;
        }

        public IFood Food { get; }

        public IGraphics2DComponent GraphicsComponent { get; }

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public int DrawOrder { get; set; }

        public bool Visible { get; set; }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public void Initialize()
        {
            throw new NotSupportedException();
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsComponent.Draw(gameTime);
        }
    }
}
