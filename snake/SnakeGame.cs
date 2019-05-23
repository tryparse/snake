using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using snake.GameComponents;
using snake.Renderers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using snake.GameEntities;
using System.Configuration;
using snake.Logging;
using SnakeGame.Shared.Settings;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.GameLogic.GameField;
using SnakeGame.Shared.GameLogic.Fruit;
using SnakeGame.Shared.GameLogic.Interfaces;
using SnakeGame.Shared.Renderers;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic;
using SnakeGame.Shared.Common.ResourceManagers;

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
        private TextureAtlas _textureRegions;

        private InputHandler _inputHandler;

        private Field _field;
        private SnakeComponent _snake;
        private IFruitManager _fruitManager;

        private GameKeys _gameKeys;
        private SnakeControls _snakeKeys;

        private IGameManager _gameManager;

        private IGameSettings _gameSettings;
        private IRenderSettings _renderSettings;
        private ITextureManager _textureManager;
        private IFruitRendererFactory _fruitRendererFactory;
        private ILogger _logger;

        public SnakeGame()
        {
            ReadSettings();

            _logger = new NLogFileLogger(_gameSettings);

            _graphics = new GraphicsDeviceManager(this)
            {

                PreferredBackBufferHeight = _gameSettings.ScreenHeight,
                PreferredBackBufferWidth = _gameSettings.ScreenWidth,
                GraphicsProfile = GraphicsProfile.HiDef
            };
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

            _renderSettings = new RenderSettings
            {
                IsDebugRenderingEnabled = _gameSettings.IsDebugRenderingEnabled,
                IsRenderingEnabled = true
            };

            _snakeKeys = new SnakeControls(Keys.Up, Keys.Down, Keys.Left, Keys.Right);
            _gameKeys = new GameKeys(Keys.P, Keys.D, Keys.R, Keys.Escape);
            
            _inputHandler = new InputHandler(this);
            Components.Add(_inputHandler);

            base.Initialize();
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

            _spriteFont = Content.Load<SpriteFont>("Fonts/debug-font");
            var texture = Content.Load<Texture2D>("Textures/Textures_sp");

            _textureRegions = new TextureAtlas("Textures", texture);

            var n = 384;

            _textureRegions.CreateRegion("Tail", 0, 0, n, n);
            _textureRegions.CreateRegion("Part", n, 0, n, n);
            _textureRegions.CreateRegion("Head", n * 2, 0, n, n);
            _textureRegions.CreateRegion("Fruit", n * 3, 0, n, n);
            _textureRegions.CreateRegion("Grass", n * 4, 0, n, n);
            _textureRegions.CreateRegion("TopLeft", 0, n * 1, n, n);
            _textureRegions.CreateRegion("TopRight", n * 1, n * 1, n, n);
            _textureRegions.CreateRegion("BottomLeft", n * 2, n * 1, n, n);
            _textureRegions.CreateRegion("BottomRight", n * 3, n * 1, n, n);
            _textureRegions.CreateRegion("Tree", n * 4, n * 1, n, n);

            var fps = new FpsCounter(this, new Vector2(GraphicsDevice.Viewport.Width - 50, 0), _spriteBatch, _spriteFont, Color.Red);

            Components.Add(fps);

            CreateGameEntities();
        }

        private void CreateGameEntities()
        {
            #region Field

            IFieldFactory fieldFactory = new FieldFactory(_gameSettings);
            _field = fieldFactory.GetRandomField(_gameSettings.MapWidth, _gameSettings.MapHeight);

            #endregion Field

            _textureManager = new TextureManager(_textureRegions);
            _fruitRendererFactory = new FruitRendererFactory(_spriteBatch, _textureManager, _renderSettings);

            #region Fruits

            _fruitManager = new FruitManager(_logger, _gameSettings, Components, _field, _fruitRendererFactory);
            _fruitManager.CreateFruit();

            #endregion Fruits

            _gameManager = new GameManager(_logger, _fruitManager);

            #region Snake

            _snake = new SnakeComponent(_logger, _gameSettings, _gameManager, _field, _field.GetRandomCell().Bounds.Center.ToVector2(), _snakeKeys)
            {
                Enabled = true
            };

            Components.Add(_snake);

            #endregion Snake

            #region Renderers

            IRenderer2D _fieldRenderer = new FieldRendererComponent(_gameSettings, _spriteBatch, _spriteFont, _renderSettings, _field, _textureRegions);
            IRenderer2D _snakeRenderer = new SnakeRendererComponent(_spriteBatch, _spriteFont, _renderSettings, _logger, _snake, _textureRegions);

            Components.Add(_snakeRenderer);
            Components.Add(_fieldRenderer);

            #endregion
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
                _snake.Enabled = !_snake.Enabled;
            }

            if (InputHandler.IsKeyPressed(_gameKeys.SwitchDebugRendering))
            {
                _renderSettings.IsDebugRenderingEnabled = !_renderSettings.IsDebugRenderingEnabled;
            }

            if (InputHandler.IsKeyPressed(_gameKeys.SwitchRendering))
            {
                _renderSettings.IsRenderingEnabled = !_renderSettings.IsRenderingEnabled;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.BackToFront, blendState: BlendState.AlphaBlend);

            base.Draw(gameTime);

            _spriteBatch.End();
        }
    }
}
