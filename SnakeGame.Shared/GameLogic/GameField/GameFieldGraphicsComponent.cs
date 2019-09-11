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
            if (_graphicsSettings.IsRenderingEnabled)
            {
                var fieldWidth = _gameField.Columns;
                var fieldHeight = _gameField.Rows;

                for (var x = 0; x < fieldWidth; x++)
                {
                    for (var y = 0; y < fieldHeight; y++)
                    {
                        var cell = _gameField.Cells[x, y];

                        switch (cell.CellType)
                        {
                            case CellType.Tree:
                            {
                                DrawGrassWithTree(cell, spriteBatch);
                                break;
                            }
                            case CellType.Grass:
                            {
                                DrawGrass(cell, spriteBatch);
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
                DebugDraw(spriteBatch);
            }
        }

        private void DebugDraw(SpriteBatch spriteBatch)
        {
            DrawTileBorders(spriteBatch);
            DrawCellIndices(spriteBatch);
        }

        private void DrawTileBorders(SpriteBatch spriteBatch)
        {
            for (var x = 0; x <= _gameField.Bounds.Width; x += _gameSettings.TileSize)
            {
                spriteBatch.DrawLine(new Vector2(x, 0), new Vector2(x, _gameField.Bounds.Height), Color.Black);
            }

            for (var y = 0; y <= _gameField.Bounds.Height; y += _gameSettings.TileSize)
            {
                spriteBatch.DrawLine(new Vector2(0, y), new Vector2(_gameField.Bounds.Width, y), Color.Black);
            }
        }

        private void DrawCellIndices(SpriteBatch spriteBatch)
        {
            var fieldWidth = _gameField.Cells.GetLength(0);
            var fieldHeight = _gameField.Cells.GetLength(1);

            for (var x = 0; x < fieldWidth; x++)
            {
                for (var y = 0; y < fieldHeight; y++)
                {
                    var cell = _gameField.Cells[x, y];

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
            }
        }

        private void DrawGrass(ICell cell, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: _graphicsSystem.TextureManager.TextureRegions["Grass"].Texture,
                destinationRectangle: cell.Bounds,
                sourceRectangle: _graphicsSystem.TextureManager.TextureRegions["Grass"].Bounds,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                layerDepth: LayerDepths.Grass);
        }

        private void DrawTree(ICell cell, SpriteBatch spriteBatch)
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

        private void DrawGrassWithTree(ICell cell, SpriteBatch spriteBatch)
        {
            DrawGrass(cell, spriteBatch);
            DrawTree(cell, spriteBatch);
        }
    }
}
