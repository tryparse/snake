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

        public GameScene(Game game, IGraphicsSystem graphicsSystem, IGameSettings gameSettings, ILogger logger) : base(
            game, graphicsSystem, gameSettings, logger)
        {
        }

        public override void Initialize()
        {
            Task.Run(() =>
            {
                _randomGenerator = new RandomGenerator(1);

                _gameFieldFactory = new GameFieldFactory(GameSettings, _randomGenerator);
                _gameField = _gameFieldFactory.GetRandomField(GameSettings.MapWidth, GameSettings.MapHeight, .8d);
                _gameFieldComponent = new GameFieldComponent(_gameField,
                    new GameFieldGraphicsComponent(_gameField, GraphicsSystem.RenderSettings, GraphicsSystem,
                        GameSettings))
                {
                    Visible = true,
                    Enabled = true
                };

                _foodManager = new FoodManager(Game, _gameField, GameSettings, GraphicsSystem, Logger);
                _foodGameComponent = _foodManager.GenerateRandomFood();
                _foodManager.Add(_foodGameComponent);

                _snake = new Snake(Logger, _gameField, GameSettings);
                _snake.Grow();
                _snakeMovementComponent =
                    new SnakeMovementTurnBased(_snake, _gameField, GameSettings,
                        new SnakeControls(Keys.Up, Keys.Down, Keys.Left, Keys.Right));
                _snakeGameComponent = new SnakeGameComponent(_snake,
                    new SnakeGraphicsComponent(_snake, GraphicsSystem, _snakeMovementComponent),
                    _snakeMovementComponent, Logger)
                {
                    Enabled = true,
                    Visible = true
                };

                _gamePoints = new GamePoints(GameSettings.RemainingLives);
                _gameManager = new GameManager(Logger, _foodManager, _snakeGameComponent, _gameField, GameSettings,
                    _gamePoints)
                {
                    Enabled = true
                };

                Game.Components.Add(_gameFieldComponent);
                Game.Components.Add(_snakeGameComponent);
                Game.Components.Add(_gameManager);

                #region UI components

                var fps = new FpsCounter(Game, new Vector2(Game.GraphicsDevice.Viewport.Width - 50, 0), GraphicsSystem.SpriteBatch,
                    GraphicsSystem.SpriteFont, Color.Black);
                Game.Components.Add(fps);

                var pointsCounterPosition = Vector2.Add(new Vector2(_gameField.Bounds.Right, _gameField.Bounds.Top),
                    new Vector2(10, 0));
                var pointsCounter = new PointsCounterComponent(Game, pointsCounterPosition, GraphicsSystem, _gamePoints);
                Game.Components.Add(pointsCounter);

                var remainingLivesPosition = Vector2.Add(new Vector2(_gameField.Bounds.Right, _gameField.Bounds.Top),
                    new Vector2(10, 25));
                var remainingLives = new RemainingLivesComponent(Game, remainingLivesPosition, GraphicsSystem, _gamePoints);
                Game.Components.Add(remainingLives);

                var debugPanel = new DebugInfoPanelComponent(Game, GraphicsSystem, GameSettings, _gameManager);
                Game.Components.Add(debugPanel);

                #endregion UI components

                IsInitialized = true;
            });
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {

        }

        public override void Unload()
        {
            Game.Components.Remove(_gameFieldComponent);
            Game.Components.Remove(_snakeGameComponent);
            Game.Components.Remove(_gameManager);
        }
    }
}