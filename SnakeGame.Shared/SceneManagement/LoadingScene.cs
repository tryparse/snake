using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings.Interfaces;

namespace SnakeGame.Shared.SceneManagement
{
    public class LoadingScene : BaseScene
    {
        private Rectangle _screenBounds;
        private readonly string _titleText = $"~~~~~SNAKE GAME~~~~~{Environment.NewLine}" +
                                             $"press enter to start";
        private readonly string LoadingText = "loading...";
        private Vector2 _textPosition;
        private Vector2 _loadingTextPosition;

        private readonly SpriteBatch _uiBatch;

        public LoadingScene(Game game, ISceneManager sceneManager, IGraphicsSystem graphicsSystem, IGameSettings gameSettings, ILogger logger, IGameKeys gameKeys) :
            base(game, sceneManager, graphicsSystem, gameSettings, logger, gameKeys)
        {
            IsLoaded = false;

            _uiBatch = new SpriteBatch(Game.GraphicsDevice);
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

                Task.Delay(300)
            };

            var result = Task.WhenAll(tasks.ToArray());

            await result;

            IsLoaded = true;
        }

        private void CalculateTextPosition()
        {
            var textSize = GraphicsSystem.SpriteFont.MeasureString(_titleText);
            _textPosition = Vector2.Add(_screenBounds.Center.ToVector2(), -Vector2.Divide(textSize, 2));

            textSize = GraphicsSystem.SpriteFont.MeasureString(LoadingText);
            _loadingTextPosition = Vector2.Add(_screenBounds.Center.ToVector2(), -Vector2.Divide(textSize, 2));
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();

            if (IsLoaded
                && InputHandler.IsKeyPressed(Keys.Enter))
            {
                SceneManager.Load(new GameScene(Game, SceneManager, GraphicsSystem, GameSettings, Logger, GameKeys));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _uiBatch.Begin();

            _uiBatch.DrawString(GraphicsSystem.SpriteFont, gameTime.TotalGameTime.ToString(),
                Vector2.One, Color.Red);

            if (!IsLoaded)
            {
                _uiBatch.DrawString(GraphicsSystem.SpriteFont, LoadingText, _loadingTextPosition, Color.White);
            }
            else
            {
                _uiBatch.DrawString(GraphicsSystem.SpriteFont, _titleText, _textPosition, Color.DarkGreen);
            }

            _uiBatch.End();
        }

        public override void Unload()
        {
            // TODO: something
        }

        private void HandleInput()
        {
            if (InputHandler.IsKeyPressed(GameKeys.Exit))
            {
                Game.Exit();
            }
        }
    }
}