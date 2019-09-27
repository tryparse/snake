using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
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
                .AddSystem(new CollisionSystem())
                .AddSystem(new RenderingSystem(GraphicsSystem))
                .Build();
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

    class CollisionSystem : UpdateSystem
    {
        public override void Update(GameTime gameTime)
        {
            
        }
    }

    class RenderingSystem : EntityDrawSystem
    {
        private IGraphicsSystem _graphicsSystem;
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Sprite> _spriteMapper;

        public RenderingSystem(IGraphicsSystem graphicsSystem) : base(new AspectBuilder().All(typeof(Sprite), typeof(Transform2)))
        {
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _spriteMapper = mapperService.GetMapper<Sprite>();
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }

    class EntityFactory
    {
        private readonly World _world;

        public EntityFactory(World world)
        {
            _world = world;
        }

        public Entity CreateSnake(Vector2 position)
        {
            var entity = _world.CreateEntity();
            entity.Attach(new Transform2());

            return entity;
        }

        public Entity CreateFood(Vector2 position)
        {
            var entity = _world.CreateEntity();
            entity.Attach(new Transform2(position));

            return entity;
        }
    }

    class NGSnakeEntity
    {
        private Transform2 _transform;

        public NGSnakeEntity(Vector2 position)
        {

        }
    }

    class SnakeMovingSystem : UpdateSystem
    {
        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
