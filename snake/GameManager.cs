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

        public static bool CheckSnakeCollision(SnakeComponent snake)
        {
            var head = snake.Parts.First();

            var tail = snake.Parts.Skip(1);

            foreach (var part in tail)
            {
                if (head.Bounds.Intersects(part.Bounds))
                {
                    snake.Enabled = false;
                    return true;
                }
            }

            return false;
        }
    }
}
