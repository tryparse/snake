using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.GameField
{
    public class GameFieldComponent : IGameFieldComponent
    {
        private readonly IGameField _gameField;
        private readonly IGraphics2DComponent _graphicsComponent;

        public GameFieldComponent(IGameField gameField, IGraphics2DComponent graphicsComponent)
        {
            _gameField = gameField;
            _graphicsComponent = graphicsComponent;
        }

        public bool Enabled { get; set; }

        public int UpdateOrder { get; set; }

        public int DrawOrder { get; set; }

        public bool Visible { get; set; }

        public IGameField GameField => _gameField;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public void Initialize()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime)
        {
            _graphicsComponent.Draw(gameTime);
        }
    }
}
