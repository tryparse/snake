﻿using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField.Cells.Interfaces;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;

namespace SnakeGame.Shared.GameLogic.GameField
{
    public class GameFieldEntity : IGameFieldEntity
    {
        private readonly IRandomGenerator _random;

        public GameFieldEntity(ICell[,] cells, IGameSettings gameSettings, IRandomGenerator random)
        {
            Cells = cells;

            Bounds = new Rectangle(0, 0, Columns * gameSettings.TileSize, Rows * gameSettings.TileSize);

            _random = random;
        }

        public Rectangle Bounds { get; }

        public int Columns => Cells.GetLength(0);

        public int Rows => Cells.GetLength(1);

        public ICell[,] Cells { get; }

        public IEnumerable<ICell> GetCells()
        {
            var result = new List<ICell>();

            foreach (var cell in Cells)
            {
                result.Add(cell);
            }

            return result;
        }

        public ICell GetRandomCell()
        {
            var x = _random.Next(0, Columns);
            var y = _random.Next(0, Rows);

            return Cells[x, y];
        }

        public ICell GetRandomCellWithoutSnake(ISnakeEntity snake)
        {
            var segments = new List<ISnakeSegment>(snake.Tail) {snake.Head};

            var snakeCells = segments
                .Select(x => GetCellByCoordinates(x.Position))
                .Where(x => x != null)
                .ToList();

            var cells = (from ICell c in Cells where !snakeCells.Contains(c) select c).ToList();

            var index = _random.Next(0, cells.Count);

            return cells[index];
        }

        public ICell GetCellByCoordinates(Vector2 position)
        {
            if (!Bounds.Contains(position))
            {
                return null;
            }

            var x = (int) Math.Floor((position.X / Bounds.Width) * Columns);
            var y = (int) Math.Floor((position.Y / Bounds.Height) * Rows);

            return Cells[x, y];
        }

        private IEnumerable<ICell> GetCellsOfType(CellType type)
        {
            var result = new List<ICell>();

            foreach (var cell in Cells)
            {
                result.Add(cell);
            }

            return result
                .Where(x => x.CellType == type)
                .ToList();
        }

        public IEnumerable<ICell> GetVisibleCells(Vector2 pov, float radius)
        {
            var visible = GetCells();

            var obstacles = GetCellsOfType(CellType.Tree)
                .Where(x => Vector2.Distance(x.Bounds.Center.ToVector2(), pov) < radius);

            visible = visible
                .Where(x => Vector2.Distance(x.Bounds.Center.ToVector2(), pov) < radius);

            var notVisible = new List<ICell>();

            foreach (var obstacle in obstacles)
            {
                var ray = new Ray2(
                    pov,
                    Vector2.Subtract(obstacle.BoundingRectangle.Center, pov));

                foreach (var cell in visible)
                {
                    if (cell.Equals(obstacle))
                    {
                        continue;
                    }

                    if  (ray.Intersects(cell.BoundingRectangle, out var rayNearDistance, out var rayFarDistance))
                    {
                        var distanceToCell = Vector2.Distance(pov, cell.BoundingRectangle.Center);
                        var distanceToObstacle = Vector2.Distance(pov, obstacle.BoundingRectangle.Center);

                        if (rayNearDistance >= 0 && rayFarDistance >= 0
                            && distanceToCell > distanceToObstacle)
                        {
                            notVisible.Add(cell);
                        }
                    }
                }

                visible = visible.Except(notVisible);
            }

            return visible;
        }

        public IEnumerable<Ray2> GetRays(Vector2 pov, float radius)
        {
            var rays = new List<Ray2>();

            var obstacles = GetCellsOfType(CellType.Tree);

            obstacles = obstacles
                .Where(x => Vector2.Distance(x.Bounds.Center.ToVector2(), pov) < radius);

            foreach (var cell in obstacles)
            {
                var ray = new Ray2(pov, Vector2.Subtract(cell.Bounds.Center.ToVector2(), pov));

                rays.Add(ray);
            }

            return rays;
        }
    }
}
