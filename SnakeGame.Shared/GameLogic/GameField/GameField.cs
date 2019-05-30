using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.GameField.Cells.Interfaces;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.GameField
{
    public class GameField : IGameField
    {
        private readonly Random _random;
        private readonly IGameSettings _gameSettings;

        public GameField(ICell[,] cells, IGameSettings gameSettings)
        {
            Cells = cells;
            _gameSettings = gameSettings;

            Bounds = new Rectangle(0, 0, Columns * _gameSettings.TileWidth, Rows * _gameSettings.TileHeight);

            _random = new Random();
        }

        public Rectangle Bounds { get; }

        public int Columns => Cells.GetLength(0);

        public int Rows => Cells.GetLength(1);

        public ICell[,] Cells { get; }

        public ICell GetRandomCell()
        {
            var x = _random.Next(0, Columns);
            var y = _random.Next(0, Rows);

            return Cells[x, y];
        }
    }
}
