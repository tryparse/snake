using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake.Interfaces
{
    public interface ISnakeMovementComponent
    {
        void Update(GameTime gameTime);

        void Reset();
    }
}
