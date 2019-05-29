using Microsoft.Xna.Framework;
using SnakeGame.Shared.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.GameField.Interfaces
{
    public interface IGameFieldComponent : IGameComponent, IUpdateable, IDrawable
    {
        IGameField GameField { get; }
    }
}
