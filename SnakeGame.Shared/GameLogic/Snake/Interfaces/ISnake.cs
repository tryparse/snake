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
        ISnakeSegment Head { get; }

        IList<ISnakeSegment> Tail { get; }

        Direction Direction { get; }

        SnakeState State { get; }

        void Initialize();

        void Reset();

        void Grow();

        void SetDirection(Direction direction);

        void SetState(SnakeState state);
    }
}
