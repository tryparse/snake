using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake.Interfaces
{
    public interface ISnakeSegment
    {
        Vector2 Position { get; }
        Size2 Size { get; }
        Direction Direction { get; }
        Rectangle Bounds { get; }
        void MoveTo(Vector2 position);
        void SetDirection(Direction direction);
    }
}
