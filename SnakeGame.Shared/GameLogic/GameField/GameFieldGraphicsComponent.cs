using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField.Cells.Interfaces;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Settings.Interfaces;
using System;
using MonoGame.Extended.Sprites;
using System.Linq;

namespace SnakeGame.Shared.GameLogic.GameField
{
    public class GameFieldGraphicsComponent : IGameFieldGraphicsComponent
    {
        // TODO: REFACTOR THIS COMPONENT!!!
        private readonly IGameFieldEntity _gameFieldEntity;
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly IGameSettings _gameSettings;
        private readonly IGraphicsSettings _graphicsSettings;

        public GameFieldGraphicsComponent(IGameFieldEntity gameField, IGraphicsSettings graphicsSettings, IGraphicsSystem graphicsSystem, IGameSettings gameSettings)
        {
            _gameFieldEntity = gameField ?? throw new ArgumentNullException(nameof(gameField));
            _graphicsSettings = graphicsSettings ?? throw new ArgumentNullException(nameof(graphicsSettings));
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
        }

        private void DebugDrawTreeCell(SpriteBatch spriteBatch, ICell cell)
        {
            // TODO: Refactor this
            var vertices = cell.Bounds.GetCorners();
            var topLeft = vertices[0].ToVector2();
            var topRight = vertices[1].ToVector2();
            var bottomRight = vertices[2].ToVector2();
            var bottomLeft = vertices[3].ToVector2();

            spriteBatch.DrawRectangle(cell.Bounds.ToRectangleF(), Color.DarkGreen, 1);
            spriteBatch.DrawLine(topLeft, bottomRight, Color.DarkGreen, 1);
            spriteBatch.DrawLine(topRight, bottomLeft, Color.DarkGreen, 1);
            DrawCellIndices(spriteBatch, cell);
        }

        private void DebugDrawGrass(SpriteBatch spriteBatch, ICell cell)
        {
            // TODO: Refactor this
            var vertices = cell.Bounds.GetCorners();
            var topLeft = vertices[0].ToVector2();
            var topRight = vertices[1].ToVector2();
            var bottomRight = vertices[2].ToVector2();
            var bottomLeft = vertices[3].ToVector2();

            var color = Color.LightSeaGreen;

            spriteBatch.DrawRectangle(cell.Bounds.ToRectangleF(), color, 1);
            spriteBatch.DrawLine(topLeft, bottomRight, color, 1);
            spriteBatch.DrawLine(topRight, bottomLeft, color, 1);
            DrawCellIndices(spriteBatch, cell);
        }

        private void DrawCellIndices(SpriteBatch spriteBatch, ICell cell)
        {
            spriteBatch.DrawString(
                        spriteFont: _graphicsSystem.DebugSpriteFont,
                        text: $"{cell.Column};{cell.Row}",
                        position: cell.Position,
                        color: Color.Black,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: 1f,
                        effects: SpriteEffects.None,
                        layerDepth: LayerDepths.Debug);
        }

        private void DrawGrassCell(SpriteBatch spriteBatch, ICell cell, Color color)
        {
            var textureRegion = _graphicsSystem.TextureManager.TextureRegions["Grass"];
            var scale = new Vector2((float)cell.Width / (float)textureRegion.Width, (float)cell.Height / (float)textureRegion.Height);

            var sprite = new Sprite(textureRegion)
            {
                Origin = Vector2.Zero,
                Position = cell.Position,
                Color = color,
                Depth = LayerDepths.Grass,
                Scale = scale
            };

            spriteBatch.Draw(sprite);
        }

        private void DrawTreeCell(SpriteBatch spriteBatch, ICell cell, Color color)
        {
            spriteBatch.Draw(
                texture: _graphicsSystem.TextureManager.TextureRegions["Tree"].Texture,
                destinationRectangle: cell.Bounds,
                sourceRectangle: _graphicsSystem.TextureManager.TextureRegions["Tree"].Bounds,
                color: color,
                rotation: 0,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                layerDepth: LayerDepths.Tree);
        }

        public void DrawGrass(SpriteBatch spriteBatch, Vector2 pointOfView, float viewRadius)
        {
            var fieldWidth = _gameFieldEntity.Columns;
            var fieldHeight = _gameFieldEntity.Rows;

            if (_graphicsSettings.IsRenderingEnabled)
            {
                var visibleGrass = _gameFieldEntity.GetVisibleCells(pointOfView, viewRadius)
                    .Where(x => x.CellType == CellType.Grass);

                var cells = _gameFieldEntity.GetCells();

                for (var x = 0; x < fieldWidth; x++)
                {
                    for (var y = 0; y < fieldHeight; y++)
                    {
                        var cell = _gameFieldEntity.Cells[x, y];

                        if (visibleGrass.Contains(cell))
                        {
                            DrawGrassCell(spriteBatch, cell, Color.White);
                        }
                        else
                        {
                            DrawGrassCell(spriteBatch, cell, Color.DarkGreen);
                        }
                    }
                }
            }
        }

        public void DrawGrassDebug(SpriteBatch spriteBatch)
        {
            var fieldWidth = _gameFieldEntity.Columns;
            var fieldHeight = _gameFieldEntity.Rows;

            if (_graphicsSettings.IsDebugRenderingEnabled)
            {
                for (var x = 0; x < fieldWidth; x++)
                {
                    for (var y = 0; y < fieldHeight; y++)
                    {
                        var cell = _gameFieldEntity.Cells[x, y];

                        switch (cell.CellType)
                        {
                            case CellType.Grass:
                                {
                                    DebugDrawGrass(spriteBatch, cell);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
            }
        }

        public void DrawTrees(SpriteBatch spriteBatch, Vector2 pointOfView, float viewRadius)
        {
            if (_graphicsSettings.IsRenderingEnabled)
            {
                var visibleTrees = _gameFieldEntity.GetVisibleCells(pointOfView, viewRadius)
                    .Where(x => x.CellType == CellType.Tree);

                var cells = _gameFieldEntity.GetCells();

                foreach (var cell in cells)
                {
                    switch (cell.CellType)
                    {
                        case CellType.Tree:
                            {
                                if (visibleTrees.Contains(cell))
                                {
                                    DrawTreeCell(spriteBatch, cell, Color.White);
                                }
                                else
                                {
                                    DrawTreeCell(spriteBatch, cell, Color.DarkGreen);
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }

        public void DrawTreesDebug(SpriteBatch spriteBatch)
        {
            if (_graphicsSettings.IsDebugRenderingEnabled)
            {
                for (var x = 0; x < _gameFieldEntity.Columns; x++)
                {
                    for (var y = 0; y < _gameFieldEntity.Rows; y++)
                    {
                        var cell = _gameFieldEntity.Cells[x, y];

                        switch (cell.CellType)
                        {
                            case CellType.Tree:
                                {
                                    DebugDrawTreeCell(spriteBatch, cell);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
            }
        }

        public void DrawBorders(SpriteBatch spriteBatch)
        {
            if (_graphicsSettings.IsRenderingEnabled)
            {
                spriteBatch.DrawRectangle(_gameFieldEntity.Bounds, Color.Black, 3);
            }
        }

        public void DrawLOSRays(SpriteBatch spriteBatch, Vector2 pov, float viewRadius)
        {
            var rays = _gameFieldEntity.GetRays(pov, viewRadius);

            foreach (var ray in rays)
            {
                spriteBatch.DrawLine(ray.Position, ray.Direction, Color.Red);
            }
        }
    }
}
