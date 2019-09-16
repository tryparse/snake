using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings.Interfaces;

namespace SnakeGame.Shared.SceneManagement
{
    class GameOverScene : BaseScene
    {
        private Rectangle _screenBounds;
        private readonly SpriteBatch _spriteBatch;

        private readonly string _gameOverText = $"GAME OVER{Environment.NewLine}" +
                                             $"press enter to try again{Environment.NewLine}" + 
                                             $"press esc to exit";

        private Vector2 _textPosition;

        public GameOverScene(Game game, ISceneManager sceneManager, IGraphicsSystem graphicsSystem,
            IGameSettings gameSettings, ILogger logger, IGameKeys gameKeys) : base(game,
            sceneManager, graphicsSystem, gameSettings, logger, gameKeys)
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public override void Load()
        {
            LoadAsync();
        }

        private async Task LoadAsync()
        {
            var tasks = new List<Task>
            {
                Task.Run(() =>
                {
                    _screenBounds = new Rectangle(0, 0, GameSettings.ScreenWidth, GameSettings.ScreenHeight);

                    CalculateTextPosition();
                }),
            };

            var result = Task.WhenAll(tasks.ToArray());

            await result;

            IsLoaded = true;
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsLoaded)
            {
                _spriteBatch.Begin();

                _spriteBatch.DrawString(GraphicsSystem.SpriteFont, _gameOverText, _textPosition, Color.ForestGreen);

                _spriteBatch.End();
            }
        }

        public override void Unload()
        {
            // TODO: something here
        }

        private void CalculateTextPosition()
        {
            var textSize = GraphicsSystem.SpriteFont.MeasureString(_gameOverText);
            _textPosition = Vector2.Add(_screenBounds.Center.ToVector2(), -Vector2.Divide(textSize, 2));
        }

        private void HandleInput()
        {
            if (InputHandler.IsKeyPressed(GameKeys.Exit))
            {
                Game.Exit();
            }

            if (IsLoaded
                && InputHandler.IsKeyPressed(Keys.Enter))
            {
                SceneManager.Load(new GameScene(Game, SceneManager, GraphicsSystem, GameSettings, Logger, GameKeys));
            }
        }
    }
}
