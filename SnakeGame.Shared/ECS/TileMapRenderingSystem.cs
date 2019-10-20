using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.ECS
{
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

                var cells = tileMap.GetCells();

                _spriteBatch.Begin();

                foreach (var cell in cells)
                {
                    _spriteBatch.DrawRectangle(cell.Rectangle, Color.Black);
                }

                _spriteBatch.End();
            }
        }
    }
}
