using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Fruit
{
    public interface IFruitManager
    {
        void CreateFruit();

        void RemoveFruit(Fruit fruit);
    }
}
