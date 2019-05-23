using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using SnakeGame.Shared.Common.ResourceManagers;
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
        private readonly ITextureManager _textureManager;
        private readonly IRenderSettings _renderSettings;

        public FruitRendererFactory(SpriteBatch spriteBatch, ITextureManager textureManager, IRenderSettings renderSettings)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            _textureManager = textureManager ?? throw new ArgumentNullException(nameof(textureManager));
            _renderSettings = renderSettings ?? throw new ArgumentNullException(nameof(renderSettings));
        }

        public FruitRendererComponent GetFruitRenderer(Fruit fruit)
        {
            return new FruitRendererComponent(fruit, _spriteBatch, _textureManager, _renderSettings);
        }
    }
}
