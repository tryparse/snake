using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.Graphics
{
    public class GraphicsSettings : IGraphicsSettings
    {
        public bool IsRenderingEnabled { get; set; }

        public bool IsDebugRenderingEnabled { get; set; }
    }
}
