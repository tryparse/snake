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
        private IGameFieldEntity _gameField;
        private IGameFieldComponent _gameFieldComponent;
        private IFoodManager _foodManager;
        private ISnakeEntity _snakeEntity;
        private ISnakeMovementComponent _snakeMovementComponent;
        private ISnakeGameComponent _snakeGameComponent;
        private IGamePoints _gamePoints;
        private IGameManager _gameManager;
        private Effect grayScaleEffect;
        private Effect shadeEffect;

        private FpsCounter _fpsCounter;
        private PointsCounterComponent _pointsCounterComponent;
        private RemainingLivesComponent _remainingLivesComponent;
        private DebugInfoPanelComponent _debugInfoPanelComponent;

        private readonly string LoadingText = "loading...";
        private Rectangle _screenBounds;

        private SpriteBatch _spriteBatch;
        private SpriteBatch _debugBatch;
        private SpriteBatch _uiBatch;

        public GameScene(Game game, ISceneManager sceneManager, IGraphicsSystem graphicsSystem, IGameSettings gameSettings, IGameLogger logger, IGameKeys gameKeys) : base(
            game, sceneManager, graphicsSystem, gameSettings, logger, gameKeys)
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _debugBatch = new SpriteBatch(Game.GraphicsDevice);
            _uiBatch = new SpriteBatch(Game.GraphicsDevice);
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
                    _randomGenerator = new RandomGenerator(2);

                    _gameFieldFactory = new GameFieldFactory(GameSettings, _randomGenerator);
                    _gameField = _gameFieldFactory.GetRandomField(GameSettings.MapWidth, GameSettings.MapHeight, 0.9d);
                    _gameFieldComponent = new GameFieldComponent(_gameField,
                        new GameFieldGraphicsComponent(_gameField, GraphicsSystem.GraphicsSettings, GraphicsSystem,
                            GameSettings));

                    _snakeEntity = new SnakeEntity(Logger, _gameField, GameSettings);
                    _snakeEntity.Grow();
                    _snakeMovementComponent =
                        new SnakeMovementTurnBased(_snakeEntity, _gameField, GameSettings,
                            new SnakeControlKeys(Keys.Up, Keys.Down, Keys.Left, Keys.Right));
                    _snakeGameComponent = new SnakeGameComponent(_snakeEntity,
                        new SnakeGraphicsComponent(_snakeEntity, GraphicsSystem, _snakeMovementComponent, _gameField),
                        _snakeMovementComponent, Logger);

                    _foodManager = new FoodManager(Game, _gameField, GameSettings, GraphicsSystem, Logger, _snakeEntity);
                    _foodManager.Add(_foodManager.GenerateRandomFood());

                    #region Common

                    _gamePoints = new GamePoints(GameSettings.RemainingLives);
                    _gameManager = new GameManager(Logger, _foodManager, _snakeGameComponent, _gameField, GameSettings,
                        _gamePoints, _snakeEntity, SceneManager, Game, GraphicsSystem, GameKeys)
                    {
                        Enabled = true
                    };

                    #endregion Common

                    grayScaleEffect = Game.Content.Load<Effect>("Effects/grayscale");
                    shadeEffect = Game.Content.Load<Effect>("Effects/dark");

                    #region UI components

                    _fpsCounter = new FpsCounter(new Vector2(Game.GraphicsDevice.Viewport.Width - 50, 0), GraphicsSystem.SpriteFont, Color.Black);

                    var pointsCounterPosition = Vector2.Add(new Vector2(_gameField.Bounds.Right, _gameField.Bounds.Top),
                        new Vector2(10, 0));
                    _pointsCounterComponent = new PointsCounterComponent(pointsCounterPosition, GraphicsSystem, _gamePoints);

                    var remainingLivesPosition = Vector2.Add(new Vector2(_gameField.Bounds.Right, _gameField.Bounds.Top),
                        new Vector2(10, 25));
                    _remainingLivesComponent = new RemainingLivesComponent(remainingLivesPosition, GraphicsSystem, _gamePoints);

                    _debugInfoPanelComponent = new DebugInfoPanelComponent(GraphicsSystem, GameSettings, _gameManager);
                    
                    #endregion UI components
                }),

                Task.Delay(500)
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
                _snakeGameComponent.Update(gameTime);
                _gameManager.Update(gameTime);

                _fpsCounter.Update(gameTime);
                _pointsCounterComponent.Update(gameTime);
                _remainingLivesComponent.Update(gameTime);
                _debugInfoPanelComponent.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!IsLoaded)
            {
                var textSize = GraphicsSystem.SpriteFont.MeasureString(LoadingText);
                var loadingTextPosition = Vector2.Add(_screenBounds.Center.ToVector2(), -Vector2.Divide(textSize, 2));

                _spriteBatch.Begin();

                _spriteBatch.DrawString(GraphicsSystem.SpriteFont, LoadingText, loadingTextPosition, Color.White);

                _spriteBatch.End();
            }
            else
            {
                var pointOfView = _snakeEntity.Head.Position;
                var viewRadius = GameSettings.TileSize * 4.5f;

                DrawGrassAndFood(pointOfView, viewRadius);

                DrawSnake(gameTime);

                DrawTrees(pointOfView, viewRadius);

                DrawUi(gameTime);

                DrawBorders(pointOfView, viewRadius);

                if (GraphicsSystem.GraphicsSettings.IsDebugRenderingEnabled)
                {
                    _debugBatch.Begin();

                    _gameFieldComponent.DrawGrassDebug(_debugBatch);
                    _gameFieldComponent.DrawTreesDebug(_debugBatch);
                    _foodManager.DebugDraw(_debugBatch);
                    _snakeGameComponent.DebugDraw(_debugBatch, pointOfView, viewRadius);
                    _gameFieldComponent.DrawLOSRays(_debugBatch, pointOfView, viewRadius);

                    _debugBatch.End();
                }
            }
        }

        private void DrawUi(GameTime gameTime)
        {
            _uiBatch.Begin();

            _debugInfoPanelComponent.Draw(_uiBatch, gameTime);
            _fpsCounter.Draw(_uiBatch, gameTime);
            _pointsCounterComponent.Draw(_uiBatch, gameTime);
            _remainingLivesComponent.Draw(_uiBatch, gameTime);

            _uiBatch.End();
        }

        private void DrawTrees(Vector2 pov, float radius)
        {
            _spriteBatch.Begin(transformMatrix: GraphicsSystem.Camera2D.GetViewMatrix());

            _gameFieldComponent.DrawTrees(_spriteBatch, pov, radius);

            _spriteBatch.End();
        }

        private void DrawSnake(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: GraphicsSystem.Camera2D.GetViewMatrix());

            _snakeGameComponent.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();
        }

        private void DrawGrassAndFood(Vector2 pointOfView, float viewRadius)
        {
            _spriteBatch.Begin(transformMatrix: GraphicsSystem.Camera2D.GetViewMatrix());

            _gameFieldComponent.DrawGrass(_spriteBatch, pointOfView, viewRadius);
            _foodManager.Draw(_spriteBatch, pointOfView, viewRadius);

            _spriteBatch.End();
        }

        private void DrawBorders(Vector2 pov, float radius)
        {
            _spriteBatch.Begin(transformMatrix: GraphicsSystem.Camera2D.GetViewMatrix());

            _gameFieldComponent.DrawBorders(_spriteBatch);

            _spriteBatch.End();
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