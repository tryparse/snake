using snake.GameEntities;
using snake.GameEntities.Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake
{
    public static class GameManager
    {
        public static SnakeComponent Snake { get; set; }

        public static void NewGame()
        {
            Snake.Reset();
        }
    }
}
