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

namespace SnakeGame.Shared.GameLogic.GameField
{
    public class GameFieldGraphicsComponent : IGraphics2DComponent
    {
        private readonly IGameField _gameField;
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly IGameSettings _gameSettings;
        private readonly IGraphicsSettings _graphicsSettings;

        public GameFieldGraphicsComponent(IGameField gameField, IGraphicsSettings graphicsSettings, IGraphicsSystem graphicsSystem, IGameSettings gameSettings)
        {
            _gameField = gameField ?? throw new ArgumentNullException(nameof(gameField));
            _graphicsSettings = graphicsSettings ?? throw new ArgumentNullException(nameof(graphicsSettings));
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var fieldWidth = _gameField.Columns;
            var fieldHeight = _gameField.Rows;

            if (_graphicsSettings.IsRenderingEnabled)
            {
                for (var x = 0; x < fieldWidth; x++)
                {
                    for (var y = 0; y < fieldHeight; y++)
                    {
                        var cell = _gameField.Cells[x, y];

                        switch (cell.CellType)
                        {
                            case CellType.Tree:
                            {
                                DrawTree(spriteBatch, cell);
                                break;
                            }
                            case CellType.Grass:
                            {
                                DrawGrass(spriteBatch, cell);
                                break;
                            }
                            case CellType.None:
                            default:
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (_graphicsSettings.IsDebugRenderingEnabled)
            {
                for (var x = 0; x < fieldWidth; x++)
                {
                    for (var y = 0; y < fieldHeight; y++)
                    {
                        var cell = _gameField.Cells[x, y];

                        switch (cell.CellType)
                        {
                            case CellType.Tree:
                                {
                                    DebugDrawTree(spriteBatch, cell);
                                    DrawCellIndices(spriteBatch, cell);
                                    break;
                                }
                            case CellType.Grass:
                                {
                                    DebugDrawGrass(spriteBatch, cell);
                                    break;
                                }
                            case CellType.None:
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
            }
        }

        private void DebugDrawTree(SpriteBatch spriteBatch, ICell cell)
        {
            var verticies = cell.Bounds.GetCorners();
            var topLeft = verticies[0].ToVector2();
            var topRight = verticies[1].ToVector2();
            var bottomRight = verticies[2].ToVector2();
            var bottomLeft = verticies[3].ToVector2();

            spriteBatch.DrawRectangle(cell.Bounds.ToRectangleF(), Color.DarkGreen, 1);
            spriteBatch.DrawLine(topLeft, bottomRight, Color.DarkGreen, 1);
            spriteBatch.DrawLine(topRight, bottomLeft, Color.DarkGreen, 1);
        }

        private void DebugDrawGrass(SpriteBatch spriteBatch, ICell cell)
        {
            var verticies = cell.Bounds.GetCorners();
            var topLeft = verticies[0].ToVector2();
            var topRight = verticies[1].ToVector2();
            var bottomRight = verticies[2].ToVector2();
            var bottomLeft = verticies[3].ToVector2();

            var color = Color.LightGreen;

            spriteBatch.DrawRectangle(cell.Bounds.ToRectangleF(), color, 1);
            spriteBatch.DrawLine(topLeft, bottomRight, color, 1);
            spriteBatch.DrawLine(topRight, bottomLeft, color, 1);
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

        private void DrawGrass(SpriteBatch spriteBatch, ICell cell)
        {
            var textureRegion = _graphicsSystem.TextureManager.TextureRegions["Grass"];
            var scale = new Vector2((float)cell.Width / (float)textureRegion.Width, (float)cell.Height / (float)textureRegion.Height);

            var sprite = new Sprite(textureRegion)
            {
                Origin = Vector2.Zero,
                Position = cell.Position,
                Color = Color.White,
                Depth = LayerDepths.Grass,
                Scale = scale
            };

            spriteBatch.Draw(sprite);
        }

        private void DrawTree(SpriteBatch spriteBatch, ICell cell)
        {
            spriteBatch.Draw(
                texture: _graphicsSystem.TextureManager.TextureRegions["Tree"].Texture,
                destinationRectangle: cell.Bounds,
                sourceRectangle: _graphicsSystem.TextureManager.TextureRegions["Tree"].Bounds,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                layerDepth: LayerDepths.Tree);
        }

        private void DrawGrassWithTree(SpriteBatch spriteBatch, ICell cell)
        {
            DrawGrass(spriteBatch, cell);
            DrawTree(spriteBatch, cell);
        }
    }
}
