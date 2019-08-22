using System;
using Microsoft.Xna.Framework;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings.Interfaces;

namespace SnakeGame.Shared.SceneManagement
{
    public abstract class BaseScene : IScene
    {
        protected readonly Game Game;
        protected readonly IGraphicsSystem GraphicsSystem;
        protected readonly IGameSettings GameSettings;
        protected readonly ILogger Logger;
        protected bool IsInitialized;

        protected BaseScene(Game game, IGraphicsSystem graphicsSystem, IGameSettings gameSettings, ILogger logger)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            GraphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            GameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void Unload();
    }
}