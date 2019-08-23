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
        protected readonly ISceneManager SceneManager;
        protected readonly IGraphicsSystem GraphicsSystem;
        protected readonly IGameSettings GameSettings;
        protected readonly ILogger Logger;
        protected readonly IGameKeys GameKeys;
        protected bool IsLoaded;

        protected BaseScene(Game game, ISceneManager sceneManager, IGraphicsSystem graphicsSystem, IGameSettings gameSettings, ILogger logger, IGameKeys gameKeys)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            SceneManager = sceneManager ?? throw new ArgumentNullException(nameof(sceneManager));
            GraphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            GameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            GameKeys = gameKeys ?? throw new ArgumentNullException(nameof(gameKeys));
        }

        public abstract void Load();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void Unload();
    }
}