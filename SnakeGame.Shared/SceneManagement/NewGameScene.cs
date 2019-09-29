using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
                .AddSystem(new TileMapRenderingSystem(GraphicsSystem))
                .Build();

            var entityFactory = new EntityFactory(_world);

            entityFactory.CreateMap(new Size(5, 5), new Size2(100, 100));
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

    class EntityFactory
    {
        private readonly World _world;

        public EntityFactory(World world)
        {
            _world = world;
        }

        public Entity CreateMap(Size mapSize, Size2 tileSize)
        {
            var entity = _world.CreateEntity();

            var tileMap = new TileMapComponent(mapSize);

            for (var x = 0; x < mapSize.Width; x++)
            {
                for (var y = 0; y < mapSize.Height; y++)
                {
                    tileMap.Grid[x, y] = new TileCell(new Transform2(x * tileSize.Width, y * tileSize.Height), new Size2(tileSize.Width, tileSize.Height));
                }
            }

            entity.Attach<TileMapComponent>(tileMap);

            return entity;
        }
    }

    class TileMapComponent
    {
        private TileCell[,] _grid;
        private Size _size;

        public TileMapComponent(Size size)
        {
            _size = size;
            _grid = new TileCell[size.Width, size.Height];
        }

        public TileCell[,] Grid => _grid;

        public IEnumerable<TileCell> GetCells()
        {
            var cells = new List<TileCell>();

            foreach (var cell in _grid)
            {
                cells.Add(cell);
            }

            return cells;
        }
    }

    class TileCell
    {
        public Size2 Size;
        public Transform2 Transform;
        private TileCellLayer[] _layers;

        public TileCell(Transform2 transform, Size2 size)
        {
            Transform = transform;
            Size = size;

            Rectangle = new RectangleF(transform.Position.X, transform.Position.Y, size.Width, size.Height);
        }

        public RectangleF Rectangle { get; }

        public TileCellLayer[] Layers => _layers;
    }

    class TileCellLayer
    {
        private Tile _tile;
    }

    class Tile
    {
        public Sprite Sprite;
    }

    class TileMapRenderingSystem : EntityDrawSystem
    {
        private ComponentMapper<TileMapComponent> _tileMapMapper;
        private readonly MonoGame.Extended.Graphics.Batcher2D _batcher2D;
        private readonly SpriteBatch _spriteBatch;

        public TileMapRenderingSystem(IGraphicsSystem graphicsSystem) : base(new AspectBuilder().One(typeof(TileMapComponent)))
        {
            _batcher2D = new MonoGame.Extended.Graphics.Batcher2D(graphicsSystem.GraphicsDevice);
            _spriteBatch = new SpriteBatch(graphicsSystem.GraphicsDevice);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _tileMapMapper = mapperService.GetMapper<TileMapComponent>();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                var tileMap = _tileMapMapper.Get(entityId);

                _spriteBatch.Begin();

                var cells = tileMap.GetCells();

                foreach (var cell in cells)
                {
                    _spriteBatch.DrawRectangle(cell.Rectangle, Color.Black);
                }

                _spriteBatch.End();
            }
        }
    }
}
