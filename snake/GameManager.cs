using snake.GameEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake
{
    public static class GameManager
    {
        public static Snake Snake { get; set; }

        public static void NewGame()
        {
            Snake.Reset();
        }
    }
}
