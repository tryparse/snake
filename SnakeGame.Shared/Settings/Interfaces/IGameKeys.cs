using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Shared.Settings.Interfaces
{
    /// <summary>
    /// Interface for encapsulating game keyboard controls
    /// </summary>
    public interface IGameKeys
    {
        Keys SwitchPause { get; }
        Keys SwitchDebugRendering { get; }
        Keys SwitchRendering { get; }
        Keys Exit { get; }
        Keys DebugInfo { get; }
    }
}
