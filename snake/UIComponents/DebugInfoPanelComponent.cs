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
        private readonly StringBuilder _stringBuilder;

        public DebugInfoPanelComponent(Game game, IGraphicsSystem graphicsSystem, IGameSettings gameSettings) : base(game)
        {
            _graphicsSystem = graphicsSystem;
            _gameSettings = gameSettings;

            _stringBuilder = new StringBuilder();
        }

        public override void Update(GameTime gameTime)
        {
            Visible = _gameSettings.IsShowDebugInfo;

            _stringBuilder.Clear();
            _stringBuilder
                .AppendLine("*** DEBUG INFO ***")
                .AppendLine($"CurrentMoveTimeInterval = {_gameSettings.CurrentMoveIntervalTime}");
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsSystem.SpriteBatch.DrawString(_graphicsSystem.DebugSpriteFont, _stringBuilder.ToString(), Vector2.One, Color.Red);
        }
    }
}
