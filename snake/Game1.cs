using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using snake.Renderers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        IFieldFactory fieldFactory;

        private Field _field;
        private Snake _snake;

        private FieldRenderer _fieldRenderer;
        private SnakeRenderer _snakeRenderer;

        private int stepTime;
        private int elapsedTime;

        private bool _isPlaying;

        Texture2D texture;
        Texture2D textureVignette;

        TextureAtlas atlas;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {

                PreferredBackBufferHeight = 512,
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

            stepTime = 1000;

            _snake = new Snake(_field.Cells[0, 0].Center);
            _isPlaying = false;

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
            _snakeRenderer = new SnakeRenderer(_snake, atlas, spriteBatch);
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

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= stepTime)
            {
                elapsedTime = elapsedTime - stepTime;
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

            _fieldRenderer.Draw(gameTime);
            _snakeRenderer.Draw(gameTime);
            //spriteBatch.Draw(textureVignette, GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
