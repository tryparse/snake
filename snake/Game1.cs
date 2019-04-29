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
using NLog;
using snake.Common;

namespace snake
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        private InputHandler _inputHandler;

        private IFieldFactory fieldFactory;

        private Field _field;
        private Snake _snake;
        private SnakeControls _controls;

        private FieldRenderer _fieldRenderer;
        private SnakeRenderer _snakeRenderer;

        private Texture2D texture;
        private Texture2D textureVignette;
        private TextureAtlas atlas;

        private ILogger _logger;

        public Game1()
        {
            _logger = LogManager.GetLogger("f");

            graphics = new GraphicsDeviceManager(this)
            {

                PreferredBackBufferHeight = 500,
                PreferredBackBufferWidth = 1024,
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
            fieldFactory = new FieldFactory();

            _field = fieldFactory.GetRandomField(10, 10);
            _controls = new SnakeControls(Keys.Up, Keys.Down, Keys.L, Keys.Right, Keys.P);
            _snake = new Snake(_logger, _field, _field.Cells[0, 0], _controls);

            _inputHandler = new InputHandler(this);
            Components.Add(_inputHandler);

            base.Initialize();
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
            texture = Content.Load<Texture2D>("Textures/Textures_sp");
            textureVignette = Content.Load<Texture2D>("Textures/Vignette_m");

            atlas = new TextureAtlas("Textures", texture);

            var n = 384;

            atlas.CreateRegion("LeftTail", 0, 0, n, n);
            atlas.CreateRegion("Center", n, 0, n, n);
            atlas.CreateRegion("Head", n * 2, 0, n, n);
            atlas.CreateRegion("Fruit", n * 3, 0, n, n);
            atlas.CreateRegion("Grass", n * 4, 0, n, n);
            atlas.CreateRegion("TopLeft", 0, n * 1, n, n);
            atlas.CreateRegion("TopRight", n * 1, n * 1, n, n);
            atlas.CreateRegion("BottomLeft", n * 2, n * 1, n, n);
            atlas.CreateRegion("BottomRight", n * 3, n * 1, n, n);
            atlas.CreateRegion("Tree", n * 4, n * 1, n, n);

            _fieldRenderer = new FieldRenderer(_field, atlas, font, spriteBatch);
            _snakeRenderer = new SnakeRenderer(_logger, _snake, atlas, spriteBatch);

            var fps = new FPSCounter(this, new Vector2(GraphicsDevice.Viewport.Width - 50, 0), spriteBatch, font, Color.Red)
            {
                DrawOrder = 100
            };
            Components.Add(fps);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _snake.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            //_fieldRenderer.Draw(gameTime);
            _snakeRenderer.Draw(gameTime);

            spriteBatch.DrawString(font, string.Join(";", _inputHandler.CurrentState.GetPressedKeys()), new Vector2(500, 0), Color.Blue);
            base.Draw(gameTime);

            for (int i = 0; i < 500; i += TileMetrics.Width / 2)
            {
                spriteBatch.DrawLine(new Vector2(0, i), new Vector2(500, i), Color.Black, 1);
            }

            for (int i = 0; i < 500; i += TileMetrics.Height / 2)
            {
                spriteBatch.DrawLine(new Vector2(i, 0), new Vector2(i, 500), Color.Black, 1);
            }

            spriteBatch.DrawLine(Vector2.Zero, _field.Cells[0, 0].Bounds.Center.ToVector2(), Color.Purple, 2);

            spriteBatch.End();
        }
    }
}
