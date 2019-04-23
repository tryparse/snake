using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;

namespace snake
{
    public class FieldRenderer : IRenderer2D
    {
        private readonly Field _field;
        private readonly TextureAtlas _textureRegions;
        private readonly SpriteFont _spriteFont;
        private readonly SpriteBatch _spriteBatch;

        private TextureRegion2D _treeTexture;

        public FieldRenderer(Field field, TextureAtlas textureRegions, SpriteFont spriteFont, SpriteBatch spriteBatch)
        {
            this._field = field;
            this._textureRegions = textureRegions;
            this._spriteFont = spriteFont;
            this._spriteBatch = spriteBatch;

            _treeTexture = _textureRegions.GetRegion("Tree");
        }

        public void Draw(GameTime gameTime)
        {
            int fieldWidth = _field.Cells.GetLength(0);
            int fieldHeight = _field.Cells.GetLength(1);

            var grassTexture = _textureRegions.GetRegion("Grass");

            for (int x = 0; x < fieldWidth; x++)
            {
                for (int y = 0; y < fieldHeight; y++)
                {
                    var cell = _field.Cells[x, y];

                    if (cell.CellType == CellType.Tree)
                    {
                        _spriteBatch.Draw(
                            texture: grassTexture.Texture,
                            destinationRectangle: cell.BoundsF.ToRectangle(),
                            sourceRectangle: grassTexture.Bounds,
                            color: Color.White,
                            rotation: 0,
                            origin: Vector2.Zero,
                            effects: SpriteEffects.None,
                            layerDepth: 0f);

                        _spriteBatch.Draw(
                            texture: _treeTexture.Texture,
                            destinationRectangle: cell.BoundsF.ToRectangle(),
                            sourceRectangle: _treeTexture.Bounds,
                            color: Color.White,
                            rotation: 0,
                            origin: Vector2.Zero,
                            effects: SpriteEffects.None,
                            layerDepth: 0.2f);
                    }
                    else if (cell.CellType == CellType.Grass)
                    {
                        _spriteBatch.Draw(
                            texture: grassTexture.Texture,
                            destinationRectangle: cell.BoundsF.ToRectangle(),
                            sourceRectangle: grassTexture.Bounds,
                            color: Color.White,
                            rotation: 0,
                            origin: Vector2.Zero,
                            effects: SpriteEffects.None,
                            layerDepth: 0f);
                    }

                    _spriteBatch.DrawString(_spriteFont, $"{x};{y}", cell.Position, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1);
                }
            }
        }
    }
}
