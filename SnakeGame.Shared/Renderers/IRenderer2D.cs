using Microsoft.Xna.Framework;
using System;

namespace SnakeGame.Shared.Renderers
{
    [Obsolete("Use IGraphics2DComponent instead")]
    public interface IRenderer2D : IGameComponent, IDrawable
    {
        IRenderSettings RenderSettings { get; }
    }
}
