using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;

namespace SnakeGame.Shared.ECS
{
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
            entity.Attach(new Transform2(position));

            return entity;
        }

        public Entity CreateFood(Vector2 position)
        {
            var entity = _world.CreateEntity();
            entity.Attach(new Transform2(position));

            return entity;
        }
    }
}
