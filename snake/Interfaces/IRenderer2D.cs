using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using snake.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake
{
    public interface IRenderer2D : IGameComponent, IDrawable
    {
        IRenderSettings RenderSettings { get; }
    }
}
