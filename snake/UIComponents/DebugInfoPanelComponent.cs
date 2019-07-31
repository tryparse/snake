using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Settings.Interfaces;

namespace snake.UIComponents
{
    public class DebugInfoPanelComponent : DrawableGameComponent
    {
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly IGameSettings _gameSettings;

        public DebugInfoPanelComponent(Game game, IGraphicsSystem graphicsSystem, IGameSettings gameSettings) : base(game)
        {
            _graphicsSystem = graphicsSystem;
            _gameSettings = gameSettings;
        }

        public override void Update(GameTime gameTime)
        {
            Visible = _gameSettings.IsShowDebugInfo;
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsSystem.SpriteBatch.DrawString(_graphicsSystem.DebugSpriteFont, "*** DEBUG INFO ***", Vector2.One, Color.DarkBlue);
        }
    }
}
