using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.Common
{
    public interface IRandomGenerator
    {
        int Next();

        int Next(int maxValue);

        int Next(int minValue, int maxValue);

        double NextDouble();
    }
}
