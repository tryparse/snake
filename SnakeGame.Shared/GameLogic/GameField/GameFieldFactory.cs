﻿using Microsoft.Xna.Framework;
using SnakeGame.Shared.GameLogic.GameField.Cells;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.GameField
{
    public class GameFieldFactory : IGameFieldFactory
    {
        private readonly IGameSettings _gameSettings;
        private readonly Random _random;

        public GameFieldFactory(IGameSettings gameSettings)
        {
            _gameSettings = gameSettings;

            _random = new Random();
        }

        public IGameField GetRandomField(int width, int height, double grassProbability)
        {
            var cells = new Cell[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var fieldTypeDice = _random.NextDouble();
                    var cellType = fieldTypeDice <= grassProbability ? CellType.Grass : CellType.Tree;

                    cells[x, y] = new Cell(
                        position: new Vector2(x * _gameSettings.TileWidth, y * _gameSettings.TileHeight),
                        column: x,
                        row: y,
                        width: _gameSettings.TileWidth,
                        height: _gameSettings.TileHeight,
                        cellType: cellType);
                }
            }

            var field = new GameField(cells, width, height);

            return field;
        }
    }
}
