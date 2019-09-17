using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic
{
    public interface IGamePoints
    {
        int Points { get; }

        int MaxPoints { get; }

        void IncrementPoints();

        int RemainingLives { get; }

        void DecrementLives();

        void Reset();
    }
}
