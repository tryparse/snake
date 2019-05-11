﻿using Microsoft.Xna.Framework;
using snake.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameEntities
{
    public class FieldFactory : IFieldFactory
    {
        public Field GetRandomField(int width, int height, double grassProbability = .8d)
        {
            var random = new Random();
            var cells = new Cell[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var fieldTypeDice = random.NextDouble();
                    var cellType = fieldTypeDice <= grassProbability ? CellType.Grass : CellType.Tree;

                    cells[x, y] = new Cell(
                        position: new Vector2(x * TileMetrics.Width, y * TileMetrics.Height),
                        indices: new Point(x, y),
                        width: TileMetrics.Width,
                        height: TileMetrics.Height,
                        cellType: cellType );
                }
            }

            var field = new Field(width, height, cells);

            return field;
        }
    }
}