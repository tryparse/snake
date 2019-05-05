//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using MonoGame.Extended;
//using MonoGame.Extended.TextureAtlases;
//using snake.GameEntities;
//using snake.Renderers;

//namespace snake
//{
//    public class FieldRenderer : IRenderer2D
//    {
//        private readonly RenderConfiguration _renderConfiguration;
//        private readonly Field _field;
//        private readonly TextureAtlas _textureRegions;
//        private readonly SpriteFont _spriteFont;
//        private readonly SpriteBatch _spriteBatch;

//        private TextureRegion2D _treeTexture;
//        private TextureRegion2D _grassTexture;

//        public FieldRenderer(RenderConfiguration configuration, Field field, TextureAtlas textureRegions, SpriteFont spriteFont, SpriteBatch spriteBatch)
//        {
//            this._renderConfiguration = configuration;
//            this._field = field;
//            this._textureRegions = textureRegions;
//            this._spriteFont = spriteFont;
//            this._spriteBatch = spriteBatch;
//            this._treeTexture = _textureRegions.GetRegion("Tree");
//            this._grassTexture = _textureRegions.GetRegion("Grass");
//        }

//        private void DebugRendering(GameTime gameTime)
//        {
//            int fieldWidth = _field.Cells.GetLength(0);
//            int fieldHeight = _field.Cells.GetLength(1);

//            for (int x = 0; x < fieldWidth; x++)
//            {
//                for (int y = 0; y < fieldHeight; y++)
//                {
//                    var cell = _field.Cells[x, y];

//                    _spriteBatch.DrawString(_spriteFont, $"{x};{y}", cell.Position, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1);
//                }
//            }
//        }

//        public void Draw(GameTime gameTime)
//        {
//            if (_renderConfiguration.IsRenderingEnabled)
//            {
//                Rendering();
//            }

//            if (_renderConfiguration.IsDebugRenderingEnabled)
//            {
//                DebugRendering(gameTime);
//            }
//        }

//        private void Rendering()
//        {
//            int fieldWidth = _field.Cells.GetLength(0);
//            int fieldHeight = _field.Cells.GetLength(1);

//            for (int x = 0; x < fieldWidth; x++)
//            {
//                for (int y = 0; y < fieldHeight; y++)
//                {
//                    var cell = _field.Cells[x, y];

//                    if (cell.CellType == CellType.Tree)
//                    {
//                        _spriteBatch.Draw(
//                            texture: _grassTexture.Texture,
//                            destinationRectangle: cell.BoundsF.ToRectangle(),
//                            sourceRectangle: _grassTexture.Bounds,
//                            color: Color.White,
//                            rotation: 0,
//                            origin: Vector2.Zero,
//                            effects: SpriteEffects.None,
//                            layerDepth: 0f);

//                        _spriteBatch.Draw(
//                            texture: _treeTexture.Texture,
//                            destinationRectangle: cell.BoundsF.ToRectangle(),
//                            sourceRectangle: _treeTexture.Bounds,
//                            color: Color.White,
//                            rotation: 0,
//                            origin: Vector2.Zero,
//                            effects: SpriteEffects.None,
//                            layerDepth: 0.2f);
//                    }
//                    else if (cell.CellType == CellType.Grass)
//                    {
//                        _spriteBatch.Draw(
//                            texture: _grassTexture.Texture,
//                            destinationRectangle: cell.BoundsF.ToRectangle(),
//                            sourceRectangle: _grassTexture.Bounds,
//                            color: Color.White,
//                            rotation: 0,
//                            origin: Vector2.Zero,
//                            effects: SpriteEffects.None,
//                            layerDepth: 0f);
//                    }
//                }
//            }
//        }
//    }
//}
