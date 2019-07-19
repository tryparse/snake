using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic
{
    public class GamePoints : IGamePoints
    {
        int _points;

        public int Points => _points;

        public void IncrementPoints()
        {
            _points++;
        }

        public void Reset()
        {
            _points = 0;
        }
    }
}
