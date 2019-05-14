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
using snake.Common;
using snake.Configuration;
using System.Configuration;
using snake.Logging;

namespace snake
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SnakeGame : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        private InputHandler _inputHandler;

        private Field _field;
        private Snake _snake;

        private GameKeys _gameKeys;
        private SnakeKeys _snakeKeys;

        private GameConfiguration _configuration;
        private RenderConfiguration _renderConfiguration;
        private ILogger _logger;

        public SnakeGame()
        {
            ReadConfiguration();

            graphics = new GraphicsDeviceManager(this)
            {

                PreferredBackBufferHeight = _configuration.ScreenHeight,
                PreferredBackBufferWidth = _configuration.ScreenWidth,
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
            _logger = new NLogFileLogger(_configuration);

            IFieldFactory fieldFactory = new FieldFactory();

            _field = fieldFactory.GetRandomField(10, 10);
            _snakeKeys = new SnakeKeys(Keys.Up, Keys.Down, Keys.Left, Keys.Right);
            _gameKeys = new GameKeys(Keys.P, Keys.D, Keys.R, Keys.Escape);

            _snake = new Snake(_logger, _field, _field.Cells[5,5].Bounds.Center.ToVector2(), _snakeKeys)
            {
                Enabled = true
            };

            GameManager.Snake = _snake;

            _inputHandler = new InputHandler(this);

            Components.Add(_inputHandler);
            Components.Add(_snake);

            base.Initialize();
        }

        private void ReadConfiguration()
        {
            _configuration = new GameConfiguration();

            bool.TryParse(ConfigurationManager.AppSettings["IsLoggingEnabled"], out var isLoggingEnabled);

            _configuration.IsLoggingEnabled = isLoggingEnabled;

            bool.TryParse(ConfigurationManager.AppSettings["IsDebugRenderingEnabled"], out var isDebugRenderingEnabled);

            _configuration.IsDebugRenderingEnabled = isDebugRenderingEnabled;

            int.TryParse(ConfigurationManager.AppSettings["ScreenWidth"], out var screenWidth);

            _configuration.ScreenWidth = screenWidth;

            int.TryParse(ConfigurationManager.AppSettings["ScreenHeight"], out var screenHeight);

            _configuration.ScreenHeight = screenHeight;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Fonts/joystix");
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

            _renderConfiguration = new RenderConfiguration
            {
                IsDebugRenderingEnabled = _configuration.IsDebugRenderingEnabled,
                IsRenderingEnabled = true
            };

            IRenderer2D _fieldRenderer = new FieldRendererComponent(spriteBatch, _renderConfiguration, _field, atlas, font);
            IRenderer2D _snakeRenderer = new SnakeRendererComponent(spriteBatch, _renderConfiguration, _logger, _snake, atlas);

            var fps = new FPSCounter(this, new Vector2(GraphicsDevice.Viewport.Width - 50, 0), spriteBatch, font, Color.Red);
        
            Components.Add(fps);
            Components.Add(_snakeRenderer);
            Components.Add(_fieldRenderer);
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
                _renderConfiguration.IsDebugRenderingEnabled = !_renderConfiguration.IsDebugRenderingEnabled;
            }

            if (InputHandler.IsKeyPressed(_gameKeys.SwitchRendering))
            {
                _renderConfiguration.IsRenderingEnabled = !_renderConfiguration.IsRenderingEnabled;
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

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.BackToFront);

            spriteBatch.DrawString(font, string.Join(";", _inputHandler.CurrentState.GetPressedKeys()), new Vector2(500, 0), Color.Blue);

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
