using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.GameField.Interfaces
{
    public interface IGameFieldFactory
    {
        IGameField GetRandomField(int width, int height, double grassProbability);
    }
}
