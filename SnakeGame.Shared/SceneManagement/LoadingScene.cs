using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings.Interfaces;

namespace SnakeGame.Shared.SceneManagement
{
    public class LoadingScene : BaseScene
    {
        private Rectangle _screenBounds;
        private const string TitleText = "SNAKE GAME";
        private const string LoadingText = "loading...";
        private Vector2 _textPosition;
        private Vector2 _loadingTextPosition;

        public LoadingScene(Game game, IGraphicsSystem graphicsSystem, IGameSettings gameSettings, ILogger logger) :
            base(game, graphicsSystem, gameSettings, logger)
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
            var textSize = GraphicsSystem.SpriteFont.MeasureString(TitleText);
            _textPosition = Vector2.Add(_screenBounds.Center.ToVector2(), -Vector2.Divide(textSize, 2));

            textSize = GraphicsSystem.SpriteFont.MeasureString(LoadingText);
            _loadingTextPosition = Vector2.Add(_screenBounds.Center.ToVector2(), -Vector2.Divide(textSize, 2));
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: something
        }

        public override void Draw(GameTime gameTime)
        {
            if (!IsInitialized)
            {
                GraphicsSystem.SpriteBatch.DrawString(GraphicsSystem.SpriteFont, LoadingText, _loadingTextPosition, Color.White);
            }
            else
            {
                GraphicsSystem.SpriteBatch.DrawString(GraphicsSystem.SpriteFont, TitleText, _textPosition, Color.DarkGreen);
            }
        }

        public override void Unload()
        {
            // TODO: something
        }
    }
}