﻿using Microsoft.Xna.Framework;
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
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.GameLogic.Snake;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic;
using SnakeGame.Shared.Common.ResourceManagers;
using SnakeGame.Shared.GameLogic.Food;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using System;
using MonoGame.Extended.Shapes;

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

        private IGraphicsSystem _renderingSystem;

        private InputHandler _inputHandler;

        private IGameFieldComponent _gameFieldComponent;
        private SnakeGameComponent _snakeGameComponent;

        private GameKeys _gameKeys;
        private SnakeControls _snakeKeys;

        private IGameManager _gameManager;
        private IFoodManager _foodManager;

        private IGameSettings _gameSettings;
        private IGraphicsSettings _renderSettings;
        private ITextureManager _textureManager;
        private ILogger _logger;

        private Vector2 _unitVector;

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
                IsRenderingEnabled = _gameSettings.IsRenderingEnabled
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

            _debugSpriteFont = Content.Load<SpriteFont>("Fonts/debug-font");
            _spriteFont = Content.Load<SpriteFont>("Fonts/joystix");
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

            var fps = new FpsCounter(this, new Vector2(GraphicsDevice.Viewport.Width - 50, 0), _spriteBatch, _spriteFont, Color.Black);

            Components.Add(fps);

            _textureManager = new TextureManager(_textureRegions);
            _renderingSystem = new GraphicsSystem(_renderSettings, _spriteBatch, _spriteFont, _debugSpriteFont, _textureManager);

            CreateGameEntities();
        }

        private void CreateGameEntities()
        {
            _unitVector = Vector2.Multiply(Vector2.UnitX, _gameSettings.TileSize);

            #region Field

            IGameFieldFactory gameFieldFactory = new GameFieldFactory(_gameSettings);
            IGameField gameField = gameFieldFactory.GetRandomField(_gameSettings.MapWidth, _gameSettings.MapHeight, .8d);
            IGraphics2DComponent graphicsComponent = new GameFieldGraphicsComponent(gameField, _renderSettings, _renderingSystem, _gameSettings);

            _gameFieldComponent = new GameFieldComponent(gameField, graphicsComponent)
            {
                Visible = true,
                Enabled = true
            };
            Components.Add(_gameFieldComponent);

            #endregion Field

            #region Food

            _foodManager = new FoodManager(this, gameField, _gameSettings, _renderingSystem);

            var food = _foodManager.GenerateFood(Vector2.Divide(_unitVector, 2));
            _foodManager.Add(food);

            #endregion Food

            #region Snake

            var movingCalculator = new MovingCalculator(_logger, gameField);

            var snake = new Snake(_logger, gameField, movingCalculator, _gameSettings);
            snake.AddSegments(2);

            var movement = new SnakeMovementTurnBased(snake, gameField, _gameSettings, _snakeKeys)
            {
                Enabled = true
            };

            var snakeGraphicsComponent = new SnakeGraphicsComponent(snake, _renderingSystem);
            _snakeGameComponent = new SnakeGameComponent(snake, snakeGraphicsComponent, movement, movingCalculator, _snakeKeys,
                _gameSettings, _logger)
            {
                Enabled = true,
                Visible = true
            };

            Components.Add(_snakeGameComponent);

            #endregion Snake

            _gameManager = new GameManager(_logger, _foodManager, snake, gameField, _gameSettings);

            Components.Add(_gameManager);
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
