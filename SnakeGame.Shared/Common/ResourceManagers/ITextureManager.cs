using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.Common.ResourceManagers
{
    public interface ITextureManager
    {
        TextureAtlas TextureRegions { get; }
    }
}
