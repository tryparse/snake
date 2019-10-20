using MonoGame.Extended;
using MonoGame.Extended.Entities;
using SnakeGame.Shared.Common;
using System;

namespace SnakeGame.Shared.ECS
{
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

        public Entity GenerateMap(Size mapSize, Size2 tileSize, IRandomGenerator randomGenerator, double grassCoeff)
        {
            if (randomGenerator == null)
            {
                throw new ArgumentNullException(nameof(randomGenerator));
            }

            if (grassCoeff > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(grassCoeff), "should be less or equal than 1");
            }

            if (grassCoeff < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(grassCoeff), "should be more than 0");
            }

            var tileMapComponent = new TileMapComponent(mapSize);

            for (var x = 0; x < mapSize.Width; x++)
            {
                for (var y = 0; y < mapSize.Height; y++)
                {
                    tileMapComponent.Grid[x, y] = new TileCell(
                            new Transform2(x * tileSize.Width, y * tileSize.Height),
                            tileSize);
                }
            }
        }
    }
}
