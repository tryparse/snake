using SnakeGame.Shared.GameLogic.GameField;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Interfaces
{
    public interface IFieldFactory
    {
        Field GetRandomField(int width, int height, double grassProbability = .8d);
    }
}
