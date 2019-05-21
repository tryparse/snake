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
using snake.GameEntities.Snake;
using snake.GameEntities.Fruit;
using snake.UIComponents;
using snake.Interfaces;
using SnakeGame.Shared.Settings;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.GameLogic.GameField;
using SnakeGame.Shared.GameLogic.Interfaces;

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

        private InputHandler _inputHandler;

        private Field _field;
        private SnakeComponent _snake;
        private Fruit _fruit;

        private GameKeys _gameKeys;
        private SnakeKeys _snakeKeys;

        private IGameSettings _gameSettings;
        private IRenderSettings _renderSettings;
        private ILogger _logger;

        public SnakeGame()
        {
            ReadSettings();

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
            _logger = new NLogFileLogger(_gameSettings);

            IFieldFactory fieldFactory = new FieldFactory(_gameSettings);

            _field = fieldFactory.GetRandomField(_gameSettings.MapWidth, _gameSettings.MapHeight);
            _snakeKeys = new SnakeKeys(Keys.Up, Keys.Down, Keys.Left, Keys.Right);
            _gameKeys = new GameKeys(Keys.P, Keys.D, Keys.R, Keys.Escape);

            _snake = new SnakeComponent(_logger, _gameSettings, _field, _field.Cells[0,0].Bounds.Center.ToVector2(), _snakeKeys)
            {
                Enabled = true
            };

            GameManager.Snake = _snake;

            var tileSize = new Vector2(_gameSettings.TileWidth, _gameSettings.TileHeight);

            _fruit = new Fruit(_field.Cells[1,1].Bounds.Center.ToVector2(), tileSize);

            _inputHandler = new InputHandler(this);

            Components.Add(_inputHandler);
            Components.Add(_snake);

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
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteFont = Content.Load<SpriteFont>("Fonts/debug-font");
            var texture = Content.Load<Texture2D>("Textures/Textures_sp");

            var atlas = new TextureAtlas("Textures", texture);

            var n = 384;

            atlas.CreateRegion("Tail", 0, 0, n, n);
            atlas.CreateRegion("Part", n, 0, n, n);
            atlas.CreateRegion("Head", n * 2, 0, n, n);
            atlas.CreateRegion("Fruit", n * 3, 0, n, n);
            atlas.CreateRegion("Grass", n * 4, 0, n, n);
            atlas.CreateRegion("TopLeft", 0, n * 1, n, n);
            atlas.CreateRegion("TopRight", n * 1, n * 1, n, n);
            atlas.CreateRegion("BottomLeft", n * 2, n * 1, n, n);
            atlas.CreateRegion("BottomRight", n * 3, n * 1, n, n);
            atlas.CreateRegion("Tree", n * 4, n * 1, n, n);

            _renderSettings = new RenderSettings
            {
                IsDebugRenderingEnabled = _gameSettings.IsDebugRenderingEnabled,
                IsRenderingEnabled = true
            };

            IRenderer2D _fieldRenderer = new FieldRendererComponent(_gameSettings, _spriteBatch, _spriteFont, _renderSettings, _field, atlas);
            IRenderer2D _snakeRenderer = new SnakeRendererComponent(_spriteBatch, _spriteFont, _renderSettings, _logger, _snake, atlas);
            IRenderer2D _fruitRenderer = new FruitRendererComponent(_fruit, _spriteBatch, atlas.GetRegion("Fruit"), _renderSettings);

            var fps = new FPSCounter(this, new Vector2(GraphicsDevice.Viewport.Width - 50, 0), _spriteBatch, _spriteFont, Color.Red);

            var keysVisualization = new KeyboardKeysComponent(this, _spriteBatch, _spriteFont, new Vector2(500, 0), new Queue<Keys>());

            Components.Add(fps);
            Components.Add(keysVisualization);
            Components.Add(_snakeRenderer);
            Components.Add(_fieldRenderer);
            Components.Add(_fruitRenderer);
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

            base.Update(gameTime);
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
