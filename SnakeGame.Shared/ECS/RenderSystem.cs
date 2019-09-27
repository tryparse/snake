using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace SnakeGame.Shared.ECS
{
    class RenderSystem : EntityDrawSystem
    {
        public RenderSystem() : base(Aspect.All())
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                throw new NotImplementedException();
            }
        }
    }
}
