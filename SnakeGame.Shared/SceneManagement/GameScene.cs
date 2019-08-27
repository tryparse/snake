using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic;
using SnakeGame.Shared.GameLogic.Food;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.GameField;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings.Interfaces;
using SnakeGame.Shared.UIComponents;

namespace SnakeGame.Shared.SceneManagement
{
    public class GameScene : BaseScene
    {
        private IRandomGenerator _randomGenerator;
        private IGameFieldFactory _gameFieldFactory;
        private IGameField _gameField;
        private IGameFieldComponent _gameFieldComponent;
        private IFoodManager _foodManager;
        private IFoodGameComponent _foodGameComponent;
        private ISnake _snake;
        private ISnakeMovementComponent _snakeMovementComponent;
        private ISnakeGameComponent _snakeGameComponent;
        private IGamePoints _gamePoints;
        private IGameManager _gameManager;

        private FpsCounter _fpsCounter;
        private PointsCounterComponent _pointsCounterComponent;
        private RemainingLivesComponent _remainingLivesComponent;
        private DebugInfoPanelComponent _debugInfoPanelComponent;

        private readonly string LoadingText = "loading...";
        private Rectangle _screenBounds;

        public GameScene(Game game, ISceneManager sceneManager, IGraphicsSystem graphicsSystem, IGameSettings gameSettings, ILogger logger, IGameKeys gameKeys) : base(
            game, sceneManager, graphicsSystem, gameSettings, logger, gameKeys)
        {
        }

        public override void Load()
        {
            _screenBounds = new Rectangle(0, 0, GameSettings.ScreenWidth, GameSettings.ScreenHeight);

            LoadAsync();
        }

        private async Task LoadAsync()
        {
            var tasks = new List<Task>
            {
                Task.Run(() =>
                {
                    _randomGenerator = new RandomGenerator(1);

                    _gameFieldFactory = new GameFieldFactory(GameSettings, _randomGenerator);
                    _gameField = _gameFieldFactory.GetRandomField(GameSettings.MapWidth, GameSettings.MapHeight, .8d);
                    _gameFieldComponent = new GameFieldComponent(_gameField,
                        new GameFieldGraphicsComponent(_gameField, GraphicsSystem.GraphicsSettings, GraphicsSystem,
                            GameSettings))
                    {
                        Visible = true,
                        Enabled = true
                    };

                    _snake = new Snake(Logger, _gameField, GameSettings);
                    _snake.Grow();
                    _snakeMovementComponent =
                        new SnakeMovementTurnBased(_snake, _gameField, GameSettings,
                            new SnakeControls(Keys.Up, Keys.Down, Keys.Left, Keys.Right));
                    _snakeGameComponent = new SnakeGameComponent(_snake,
                        new SnakeGraphicsComponent(_snake, GraphicsSystem, _snakeMovementComponent, _gameField),
                        _snakeMovementComponent, Logger)
                    {
                        Enabled = true,
                        Visible = true
                    };

                    _foodManager = new FoodManager(Game, _gameField, GameSettings, GraphicsSystem, Logger, _snake);
                    _foodGameComponent = _foodManager.GenerateRandomFood();

                    #region Common

                    _gamePoints = new GamePoints(GameSettings.RemainingLives);
                    _gameManager = new GameManager(Logger, _foodManager, _snakeGameComponent, _gameField, GameSettings,
                        _gamePoints)
                    {
                        Enabled = true
                    };

                    #endregion Common

                    #region UI components

                    _fpsCounter = new FpsCounter(Game, new Vector2(Game.GraphicsDevice.Viewport.Width - 50, 0), GraphicsSystem.SpriteBatch,
                        GraphicsSystem.SpriteFont, Color.Black);

                    var pointsCounterPosition = Vector2.Add(new Vector2(_gameField.Bounds.Right, _gameField.Bounds.Top),
                        new Vector2(10, 0));
                    _pointsCounterComponent = new PointsCounterComponent(Game, pointsCounterPosition, GraphicsSystem, _gamePoints);

                    var remainingLivesPosition = Vector2.Add(new Vector2(_gameField.Bounds.Right, _gameField.Bounds.Top),
                        new Vector2(10, 25));
                    _remainingLivesComponent = new RemainingLivesComponent(Game, remainingLivesPosition, GraphicsSystem, _gamePoints);

                    _debugInfoPanelComponent = new DebugInfoPanelComponent(Game, GraphicsSystem, GameSettings, _gameManager);
                    
                    #endregion UI components
                })
            };

            var result = Task.WhenAll(tasks.ToArray());

            await result;

            IsLoaded = true;

            Game.Components.Add(_gameFieldComponent);
            Game.Components.Add(_snakeGameComponent);
            _foodManager.Add(_foodGameComponent);
            Game.Components.Add(_gameManager);

            Game.Components.Add(_fpsCounter);
            Game.Components.Add(_pointsCounterComponent);
            Game.Components.Add(_remainingLivesComponent);
            Game.Components.Add(_debugInfoPanelComponent);
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
        }

        public override void Draw(GameTime gameTime)
        {
            if (!IsLoaded)
            {
                var textSize = GraphicsSystem.SpriteFont.MeasureString(LoadingText);
                var loadingTextPosition = Vector2.Add(_screenBounds.Center.ToVector2(), -Vector2.Divide(textSize, 2));

                GraphicsSystem.SpriteBatch.DrawString(GraphicsSystem.SpriteFont, LoadingText, loadingTextPosition, Color.White);
            }
        }

        public override void Unload()
        {
            Game.Components.Remove(_gameFieldComponent);
            Game.Components.Remove(_snakeGameComponent);
            _foodManager.Reset();
            Game.Components.Remove(_gameManager);
            Game.Components.Remove(_fpsCounter);
            Game.Components.Remove(_pointsCounterComponent);
            Game.Components.Remove(_remainingLivesComponent);
            Game.Components.Remove(_debugInfoPanelComponent);
        }

        private void HandleInput()
        {
            if (InputHandler.IsKeyPressed(GameKeys.Exit))
            {
                SceneManager.Load(new LoadingScene(Game, SceneManager, GraphicsSystem, GameSettings, Logger, GameKeys));
            }

            if (InputHandler.IsKeyPressed(GameKeys.SwitchPause))
            {
                _gameManager.TogglePause();
            }

            if (InputHandler.IsKeyPressed(GameKeys.SwitchDebugRendering))
            {
                GraphicsSystem.GraphicsSettings.ToggleDebugRenderingEnabled();
            }

            if (InputHandler.IsKeyPressed(GameKeys.SwitchRendering))
            {
                GraphicsSystem.GraphicsSettings.ToggleRenderingEnabled();
            }

            if (InputHandler.IsKeyPressed(GameKeys.DebugInfo))
            {
                GameSettings.ToggleDebugInfo();
            }
        }
    }
}