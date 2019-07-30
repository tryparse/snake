using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using snake.GameComponents;
using snake.GameEntities;
using snake.Logging;
using snake.UIComponents;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.Common.ResourceManagers;
using SnakeGame.Shared.GameLogic;
using SnakeGame.Shared.GameLogic.Food;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.GameField;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings.Implementation;
using SnakeGame.Shared.Settings.Interfaces;
using System.Configuration;

namespace snake
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SnakeGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private SpriteFont _debugSpriteFont;
        private TextureAtlas _textureRegions;

        private IGraphicsSystem _graphicsSystem;

        private InputHandler _inputHandler;

        private IGameFieldComponent _gameFieldComponent;
        private SnakeGameComponent _snakeGameComponent;

        private GameKeys _gameKeys;
        private SnakeControls _snakeKeys;

        private IGameManager _gameManager;
        private IFoodManager _foodManager;

        private IGameSettings _gameSettings;
        private IGraphicsSettings _graphicsSettings;
        private ITextureManager _textureManager;
        private readonly ILogger _logger;

        private Camera2D _camera;

        public SnakeGame()
        {
            ReadSettings();

            _logger = new NLogFileLogger(_gameSettings);

            _graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };

            ApplyScreenChanges();

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _logger.Debug("Game.Initialize()");

            _graphicsSettings = new GraphicsSettings
            {
                IsDebugRenderingEnabled = _gameSettings.IsDebugRenderingEnabled,
                IsRenderingEnabled = _gameSettings.IsRenderingEnabled
            };

            _snakeKeys = new SnakeControls(Keys.Up, Keys.Down, Keys.Left, Keys.Right);
            _gameKeys = new GameKeys(Keys.P, Keys.D, Keys.R, Keys.Escape);
            
            _inputHandler = new InputHandler(this);
            Components.Add(_inputHandler);

            _camera = new Camera2D(GraphicsDevice);

            base.Initialize();
        }

        private void ApplyScreenChanges()
        {
            _graphics.PreferredBackBufferHeight = _gameSettings.ScreenHeight;
            _graphics.PreferredBackBufferWidth = _gameSettings.ScreenWidth;
            _graphics.IsFullScreen = _gameSettings.IsFullScreen;
            _graphics.ApplyChanges();
        }

        private void ReadSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;

            _gameSettings = new GameSettings();
            _gameSettings.ReadFromApplicationSettings(appSettings);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _logger.Debug("Game.LoadContent()");

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _debugSpriteFont = Content.Load<SpriteFont>("Fonts/debug-font");
            _spriteFont = Content.Load<SpriteFont>("Fonts/joystix");
            var texture = Content.Load<Texture2D>("Textures/Textures_sp");

            _textureRegions = new TextureAtlas("Textures", texture);

            const int TEXTURE_ATLAS_CONSTANT = 384;

            _textureRegions.CreateRegion("Tail", 0, 0, TEXTURE_ATLAS_CONSTANT, TEXTURE_ATLAS_CONSTANT);
            _textureRegions.CreateRegion("Part", TEXTURE_ATLAS_CONSTANT, 0, TEXTURE_ATLAS_CONSTANT, TEXTURE_ATLAS_CONSTANT);
            _textureRegions.CreateRegion("Head", TEXTURE_ATLAS_CONSTANT * 2, 0, TEXTURE_ATLAS_CONSTANT, TEXTURE_ATLAS_CONSTANT);
            _textureRegions.CreateRegion("Fruit", TEXTURE_ATLAS_CONSTANT * 3, 0, TEXTURE_ATLAS_CONSTANT, TEXTURE_ATLAS_CONSTANT);
            _textureRegions.CreateRegion("Grass", TEXTURE_ATLAS_CONSTANT * 4, 0, TEXTURE_ATLAS_CONSTANT, TEXTURE_ATLAS_CONSTANT);
            _textureRegions.CreateRegion("TopLeft", 0, TEXTURE_ATLAS_CONSTANT * 1, TEXTURE_ATLAS_CONSTANT, TEXTURE_ATLAS_CONSTANT);
            _textureRegions.CreateRegion("TopRight", TEXTURE_ATLAS_CONSTANT * 1, TEXTURE_ATLAS_CONSTANT * 1, TEXTURE_ATLAS_CONSTANT, TEXTURE_ATLAS_CONSTANT);
            _textureRegions.CreateRegion("BottomLeft", TEXTURE_ATLAS_CONSTANT * 2, TEXTURE_ATLAS_CONSTANT * 1, TEXTURE_ATLAS_CONSTANT, TEXTURE_ATLAS_CONSTANT);
            _textureRegions.CreateRegion("BottomRight", TEXTURE_ATLAS_CONSTANT * 3, TEXTURE_ATLAS_CONSTANT * 1, TEXTURE_ATLAS_CONSTANT, TEXTURE_ATLAS_CONSTANT);
            _textureRegions.CreateRegion("Tree", TEXTURE_ATLAS_CONSTANT * 4, TEXTURE_ATLAS_CONSTANT * 1, TEXTURE_ATLAS_CONSTANT, TEXTURE_ATLAS_CONSTANT);

            _textureManager = new TextureManager(_textureRegions);
            _graphicsSystem = new GraphicsSystem(_graphicsSettings, _spriteBatch, _spriteFont, _debugSpriteFont, _textureManager);

            CreateGameEntities();
        }

        private void CreateGameEntities()
        {
            var random = new RandomGenerator();

            var gamePoints = new GamePoints(_gameSettings.RemainingLives);

            #region Field

            IGameFieldFactory gameFieldFactory = new GameFieldFactory(_gameSettings, random);
            IGameField gameField = gameFieldFactory.GetRandomField(_gameSettings.MapWidth, _gameSettings.MapHeight, .8d);
            IGraphics2DComponent graphicsComponent = new GameFieldGraphicsComponent(gameField, _graphicsSettings, _graphicsSystem, _gameSettings);

            _gameFieldComponent = new GameFieldComponent(gameField, graphicsComponent)
            {
                Visible = true,
                Enabled = true
            };
            Components.Add(_gameFieldComponent);

            #endregion Field

            #region Food

            _foodManager = new FoodManager(this, gameField, _gameSettings, _graphicsSystem, _logger);

            var food = _foodManager.GenerateRandomFood();
            _foodManager.Add(food);

            #endregion Food

            #region Snake

            var movingCalculator = new MovingCalculator(_logger, gameField);

            var snake = new Snake(_logger, gameField, movingCalculator, _gameSettings);
            snake.Grow();

            var movement = new SnakeMovementTurnBased(snake, gameField, _gameSettings, _snakeKeys)
            {
                Enabled = true
            };

            var snakeGraphicsComponent = new SnakeGraphicsComponent(snake, _graphicsSystem, movement);
            _snakeGameComponent = new SnakeGameComponent(snake, snakeGraphicsComponent, movement, _logger)
            {
                Enabled = true,
                Visible = true
            };

            Components.Add(_snakeGameComponent);

            #endregion Snake

            _gameManager = new GameManager(_logger, _foodManager, _snakeGameComponent, gameField, _gameSettings, gamePoints)
            {
                Enabled = true
            };

            Components.Add(_gameManager);

            #region UI components

            var fps = new FpsCounter(this, new Vector2(GraphicsDevice.Viewport.Width - 50, 0), _spriteBatch,
                _spriteFont, Color.Black);
            Components.Add(fps);

            var pointsCounterPosition = Vector2.Add(new Vector2(gameField.Bounds.Right, gameField.Bounds.Top),
                new Vector2(10, 0));
            var pointsCounter = new PointsCounterComponent(this, pointsCounterPosition, _graphicsSystem, gamePoints);
            Components.Add(pointsCounter);

            var remainingLivesPosition = Vector2.Add(new Vector2(gameField.Bounds.Right, gameField.Bounds.Top),
                new Vector2(10, 25));
            var remainingLives = new RemainingLivesComponent(this, remainingLivesPosition, _graphicsSystem, gamePoints);
            Components.Add(remainingLives);

            #endregion UI components
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();

            base.Update(gameTime);
        }

        private void HandleInput()
        {
            if (InputHandler.IsKeyPressed(_gameKeys.Exit))
            {
                Exit();
            }

            if (InputHandler.IsKeyPressed(_gameKeys.SwitchPause))
            {
                _snakeGameComponent.Enabled = !_snakeGameComponent.Enabled;
            }

            if (InputHandler.IsKeyPressed(_gameKeys.SwitchDebugRendering))
            {
                _graphicsSettings.IsDebugRenderingEnabled = !_graphicsSettings.IsDebugRenderingEnabled;
            }

            if (InputHandler.IsKeyPressed(_gameKeys.SwitchRendering))
            {
                _graphicsSettings.IsRenderingEnabled = !_graphicsSettings.IsRenderingEnabled;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.BackToFront, blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());

            base.Draw(gameTime);

            _spriteBatch.End();
        }
    }
}
