using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
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

        private SpriteBatch _gameSpriteBatch;
        private SpriteBatch _uiSpriteBatch;
        private SpriteBatch _debugSpriteBatch;

        public GameScene(Game game, ISceneManager sceneManager, IGraphicsSystem graphicsSystem, IGameSettings gameSettings, ILogger logger, IGameKeys gameKeys) : base(
            game, sceneManager, graphicsSystem, gameSettings, logger, gameKeys)
        {
            _gameSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _uiSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _debugSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
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
                            GameSettings));

                    _snake = new Snake(Logger, _gameField, GameSettings);
                    _snake.Grow();
                    _snakeMovementComponent =
                        new SnakeMovementTurnBased(_snake, _gameField, GameSettings,
                            new SnakeControls(Keys.Up, Keys.Down, Keys.Left, Keys.Right));
                    _snakeGameComponent = new SnakeGameComponent(_snake,
                        new SnakeGraphicsComponent(_snake, GraphicsSystem, _snakeMovementComponent, _gameField),
                        _snakeMovementComponent, Logger);

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

                    _fpsCounter = new FpsCounter(new Vector2(Game.GraphicsDevice.Viewport.Width - 50, 0), GraphicsSystem.SpriteFont, Color.Black, GraphicsSystem);

                    var pointsCounterPosition = Vector2.Add(new Vector2(_gameField.Bounds.Right, _gameField.Bounds.Top),
                        new Vector2(10, 0));
                    _pointsCounterComponent = new PointsCounterComponent(pointsCounterPosition, GraphicsSystem, _gamePoints);

                    var remainingLivesPosition = Vector2.Add(new Vector2(_gameField.Bounds.Right, _gameField.Bounds.Top),
                        new Vector2(10, 25));
                    _remainingLivesComponent = new RemainingLivesComponent(remainingLivesPosition, GraphicsSystem, _gamePoints);

                    _debugInfoPanelComponent = new DebugInfoPanelComponent(GraphicsSystem, GameSettings, _gameManager);
                    
                    #endregion UI components
                })
            };

            await Task.WhenAll(tasks.ToArray());

            IsLoaded = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsLoaded)
            {
                HandleInput();

                _gameFieldComponent.Update(gameTime);

                _fpsCounter.Update(gameTime);
                _pointsCounterComponent.Update(gameTime);
                _remainingLivesComponent.Update(gameTime);
                _debugInfoPanelComponent.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _gameSpriteBatch.Begin(samplerState: SamplerState.PointClamp,
                sortMode: SpriteSortMode.Texture, transformMatrix: GraphicsSystem.Camera2D.GetViewMatrix(),
                depthStencilState: DepthStencilState.Default);
            _uiSpriteBatch.Begin(samplerState: SamplerState.PointClamp,
                sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend,
                transformMatrix: GraphicsSystem.Camera2D.GetViewMatrix());

            if (!IsLoaded)
            {
                var textSize = GraphicsSystem.SpriteFont.MeasureString(LoadingText);
                var loadingTextPosition = Vector2.Add(_screenBounds.Center.ToVector2(), -Vector2.Divide(textSize, 2));

                _uiSpriteBatch.DrawString(GraphicsSystem.SpriteFont, LoadingText, loadingTextPosition, Color.White);
            }
            else
            {
                _gameFieldComponent.Draw(_gameSpriteBatch, gameTime);
            }

            _gameSpriteBatch.End();
            _uiSpriteBatch.End();
        }

        public override void Unload()
        {
            
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