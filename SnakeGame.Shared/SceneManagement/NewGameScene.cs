using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using SnakeGame.Shared.ECS;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Logging;
using SnakeGame.Shared.Settings.Interfaces;

namespace SnakeGame.Shared.SceneManagement
{
    class NewGameScene : BaseScene
    {
        private World _world;

        public NewGameScene(Game game, ISceneManager sceneManager, IGraphicsSystem graphicsSystem,
            IGameSettings gameSettings, IGameLogger logger, IGameKeys gameKeys) : base(game, sceneManager,
            graphicsSystem, gameSettings, logger, gameKeys)
        {
        }

        public override void Load()
        {
            _world = new WorldBuilder()
                .AddSystem(new TileMapRenderingSystem(GraphicsSystem))
                .Build();

            var entityFactory = new EntityFactory(_world);

            entityFactory.CreateMap(new Size(10, 10), new Size2(50, 50));
        }

        public override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
