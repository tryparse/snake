using SnakeGame.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake.Interfaces
{
    public interface ISnake
    {
        LinkedList<ISnakeSegment> Segments { get; }

        Direction Direction { get; }

        SnakeState State { get; }

        void Reset();

        void AddSegments(uint count);

        void SetDirection(Direction direction);

        void SetState(SnakeState state);
    }
}
