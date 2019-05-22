using SnakeGame.Shared.GameLogic.Fruit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.Renderers
{
    public interface IFruitRendererFactory
    {
        FruitRendererComponent GetFruitRenderer(Fruit fruit);
    }
}
