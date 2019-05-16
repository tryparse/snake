using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.Interfaces
{
    public interface IRenderSettings
    {
        bool IsRenderingEnabled { get; set; }

        bool IsDebugRenderingEnabled { get; set; }
    }
}
