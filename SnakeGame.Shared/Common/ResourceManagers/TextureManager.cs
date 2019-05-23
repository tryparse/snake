using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.Common.ResourceManagers
{
    public class TextureManager : ITextureManager
    {
        private readonly TextureAtlas _textureAtlas;

        public TextureManager(TextureAtlas textureAtlas)
        {
            _textureAtlas = textureAtlas;
        }

        public TextureAtlas TextureRegions => _textureAtlas;
    }
}
