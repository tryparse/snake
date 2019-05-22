using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using SnakeGame.Shared.GameLogic.Fruit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.Renderers
{
    public class FruitRendererFactory : IFruitRendererFactory
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly TextureRegion2D _textureRegion2D;
        private readonly IRenderSettings _renderSettings;

        public FruitRendererFactory(SpriteBatch spriteBatch, TextureRegion2D textureRegion2D, IRenderSettings renderSettings)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            _textureRegion2D = textureRegion2D ?? throw new ArgumentNullException(nameof(textureRegion2D));
            _renderSettings = renderSettings ?? throw new ArgumentNullException(nameof(renderSettings));
        }

        public FruitRendererComponent GetFruitRenderer(Fruit fruit)
        {
            return new FruitRendererComponent(fruit, _spriteBatch, _textureRegion2D, _renderSettings);
        }
    }
}
