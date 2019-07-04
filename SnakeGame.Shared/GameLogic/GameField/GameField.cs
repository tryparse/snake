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
        private readonly Vector2 _unitVector;

        public GameField(ICell[,] cells, IGameSettings gameSettings)
        {
            Cells = cells;
            _gameSettings = gameSettings;

            _unitVector = new Vector2(_gameSettings.TileWidth, _gameSettings.TileHeight);

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

        public ICell GetCellByCoordinates(Vector2 position)
        {
            if (!Bounds.Contains(position))
            {
                throw new InvalidOperationException();
            }

            var x = (int)Math.Floor((Bounds.Width / position.X) / _unitVector.X);
            var y = (int)Math.Floor((Bounds.Height / position.Y) / _unitVector.Y);

            return Cells[x, y];
        }
    }
}
