using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake.Interfaces
{
    public interface ISnakeGameComponent : IGameComponent, IUpdateable, IDrawable
    {
        ISnake Snake { get; }
        ISnakeMovementComponent SnakeMovementComponent { get; }

        void Reset();

        new bool Enabled { get; set; }
    }
}
