using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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

        public LoadingScene(Game game, ISceneManager sceneManager, IGraphicsSystem graphicsSystem, IGameSettings gameSettings, ILogger logger, IGameKeys gameKeys) :
            base(game, sceneManager, graphicsSystem, gameSettings, logger, gameKeys)
        {
            IsInitialized = false;
        }

        public override void Initialize()
        {
            var task = Task.Run(() =>
            {
                _screenBounds = new Rectangle(0, 0, GameSettings.ScreenWidth, GameSettings.ScreenHeight);

                CalculateTextPosition();

                Thread.Sleep(1000);

                IsInitialized = true;
            });
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
            if (IsInitialized
                && InputHandler.IsKeyPressed(Keys.Enter))
            {
                SceneManager.Load(new GameScene(Game, SceneManager, GraphicsSystem, GameSettings, Logger, GameKeys));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!IsInitialized)
            {
                GraphicsSystem.SpriteBatch.DrawString(GraphicsSystem.SpriteFont, LoadingText, _loadingTextPosition, Color.White);
            }
            else
            {
                GraphicsSystem.SpriteBatch.DrawString(GraphicsSystem.SpriteFont, _titleText, _textPosition, Color.DarkGreen);
            }
        }

        public override void Unload()
        {
            // TODO: something
        }
    }
}