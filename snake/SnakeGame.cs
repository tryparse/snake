using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using snake.Logging;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.Common.ResourceManagers;
using SnakeGame.Shared.GameLogic;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.SceneManagement;
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
        private SpriteFont _spriteFont;
        private SpriteFont _debugSpriteFont;
        private TextureAtlas _textureRegions;

        private IGraphicsSystem _graphicsSystem;

        private InputHandler _inputHandler;

        private IGameKeys _gameKeys;

        private IGameSettings _gameSettings;
        private IGraphicsSettings _graphicsSettings;
        private ITextureManager _textureManager;
        private readonly IGameLogger _logger;

        private ISceneManager _sceneManager;

        private OrthographicCamera _camera;

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

            _gameKeys = new GameKeys(Keys.Space, Keys.D, Keys.R, Keys.Escape, Keys.OemTilde);
            
            _inputHandler = new InputHandler(this);
            Components.Add(_inputHandler);

            _camera = new OrthographicCamera(GraphicsDevice);

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

            LoadFonts();

            LoadTextures();

            _textureManager = new TextureManager(_textureRegions);
            _graphicsSystem = new GraphicsSystem(_graphicsSettings, Content, _spriteFont, _debugSpriteFont, _textureManager, _camera, GraphicsDevice);

            _sceneManager = new SceneManager(this);

            Components.Add(_sceneManager);

            _sceneManager.Load(new LoadingScene(this, _sceneManager, _graphicsSystem, _gameSettings, _logger, _gameKeys));
        }

        private void LoadFonts()
        {
            _debugSpriteFont = Content.Load<SpriteFont>("Fonts/debug-font");
            _spriteFont = Content.Load<SpriteFont>("Fonts/joystix");
        }

        private void LoadTextures()
        {
            var texture = Content.Load<Texture2D>("Textures/Textures_sp");

            _textureRegions = new TextureAtlas("Textures", texture);

            const int textureAtlasConstant = 384;

            _textureRegions.CreateRegion("Tail", 0, 0, textureAtlasConstant, textureAtlasConstant);
            _textureRegions.CreateRegion("Part", textureAtlasConstant, 0, textureAtlasConstant, textureAtlasConstant);
            _textureRegions.CreateRegion("Head", textureAtlasConstant * 2, 0, textureAtlasConstant, textureAtlasConstant);
            _textureRegions.CreateRegion("Fruit", textureAtlasConstant * 3, 0, textureAtlasConstant,
                textureAtlasConstant);
            _textureRegions.CreateRegion("Grass", textureAtlasConstant * 4, 0, textureAtlasConstant,
                textureAtlasConstant);
            _textureRegions.CreateRegion("TopLeft", 0, textureAtlasConstant * 1, textureAtlasConstant,
                textureAtlasConstant);
            _textureRegions.CreateRegion("TopRight", textureAtlasConstant * 1, textureAtlasConstant * 1,
                textureAtlasConstant, textureAtlasConstant);
            _textureRegions.CreateRegion("BottomLeft", textureAtlasConstant * 2, textureAtlasConstant * 1,
                textureAtlasConstant, textureAtlasConstant);
            _textureRegions.CreateRegion("BottomRight", textureAtlasConstant * 3, textureAtlasConstant * 1,
                textureAtlasConstant, textureAtlasConstant);
            _textureRegions.CreateRegion("Tree", textureAtlasConstant * 4, textureAtlasConstant * 1, textureAtlasConstant,
                textureAtlasConstant);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _logger.Debug("SnakeGame.UnloadContent()");
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
