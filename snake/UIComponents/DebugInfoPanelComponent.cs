using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Settings.Interfaces;

namespace snake.UIComponents
{
    public class DebugInfoPanelComponent : DrawableGameComponent
    {
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly IGameSettings _gameSettings;
        private readonly IGameManager _gameManager;
        private readonly StringBuilder _stringBuilder;

        public DebugInfoPanelComponent(Game game, IGraphicsSystem graphicsSystem, IGameSettings gameSettings, IGameManager gameManager) : base(game)
        {
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));

            _stringBuilder = new StringBuilder();
        }

        public override void Update(GameTime gameTime)
        {
            Visible = _gameSettings.IsShowDebugInfo;

            _stringBuilder.Clear();
            _stringBuilder
                .AppendLine("*** DEBUG INFO ***")
                .AppendLine($"CurrentMoveTimeInterval = {_gameSettings.CurrentMoveIntervalTime}")
                .AppendLine($"IsPaused = {_gameManager.IsPaused}");
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsSystem.SpriteBatch.DrawString(_graphicsSystem.DebugSpriteFont, _stringBuilder.ToString(), Vector2.One, Color.Red);
        }
    }
}
