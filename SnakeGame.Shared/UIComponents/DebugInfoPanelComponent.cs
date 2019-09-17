using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using SnakeGame.Shared.GameLogic;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Settings.Interfaces;

namespace SnakeGame.Shared.UIComponents
{
    public class DebugInfoPanelComponent : IUiComponent
    {
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly IGameSettings _gameSettings;
        private readonly IGameManager _gameManager;
        private readonly StringBuilder _stringBuilder;

        public DebugInfoPanelComponent(IGraphicsSystem graphicsSystem, IGameSettings gameSettings, IGameManager gameManager)
        {
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));

            _stringBuilder = new StringBuilder();
        }

        public void Update(GameTime gameTime)
        {
            if (!_gameSettings.IsShowDebugInfo)
            {
                return;
            }

            _stringBuilder.Clear();
            _stringBuilder
                .AppendLine("*** DEBUG INFO ***")
                .AppendLine($"CurrentMoveTimeInterval = {_gameSettings.CurrentMoveIntervalTime}")
                .AppendLine($"IsPaused = {_gameManager.IsPaused}")
                .AppendLine($"IsDebugRenderingEnabled = {_graphicsSystem.GraphicsSettings.IsDebugRenderingEnabled}");
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!_gameSettings.IsShowDebugInfo)
            {
                return;;
            }

            spriteBatch.DrawString(_graphicsSystem.DebugSpriteFont, _stringBuilder.ToString(), Vector2.One, Color.Red);
        }
    }
}
