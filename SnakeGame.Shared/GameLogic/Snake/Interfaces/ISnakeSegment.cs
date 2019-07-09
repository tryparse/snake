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
        Vector2? TargetPosition { get; }
        Vector2? SourcePosition { get; }
        Size2 Size { get; }
        Direction Direction { get; }
        Rectangle Bounds { get; }
        void MoveTo(Vector2 position);
        void SetTargetPosition(Vector2? targetPosition);
        void SetSourcePosition(Vector2? sourcePosition);
        void SetDirection(Direction direction);
    }
}
