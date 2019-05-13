using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.TextureAtlases;
using snake.Common;
using snake.GameEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.Renderers
{
    class FieldRendererComponent : IRenderer2D
    {
        private readonly RenderConfiguration _renderConfiguration;
        private readonly Field _field;
        private readonly TextureAtlas _textureRegions;
        private readonly SpriteFont _spriteFont;
        private readonly SpriteBatch _spriteBatch;

        private int _drawOrder;
        private bool _isVisible;

        private readonly TextureRegion2D _treeTexture;
        private readonly TextureRegion2D _grassTexture;

        public FieldRendererComponent(SpriteBatch spriteBatch, RenderConfiguration configuration, Field field, TextureAtlas textureRegions, SpriteFont spriteFont, int drawOrder = 0)
        {
            this._renderConfiguration = configuration;
            this._field = field;
            this._textureRegions = textureRegions;
            this._spriteFont = spriteFont;
            this._spriteBatch = spriteBatch;
            this._treeTexture = _textureRegions.GetRegion("Tree");
            this._grassTexture = _textureRegions.GetRegion("Grass");
            this._drawOrder = drawOrder;
            this._isVisible = true;
        }

        public int DrawOrder
        {
            get => _drawOrder;
            set
            {
                if (!value.Equals(_drawOrder))
                {
                    DrawOrderChanged?.Invoke(this, new EventArgs());
                }
                _drawOrder = value;
            }
        }

        public bool Visible
        {
            get => _isVisible;
            set
            {
                if (!value.Equals(_isVisible))
                {
                    VisibleChanged?.Invoke(this, new EventArgs());
                }
                _isVisible = value;
            }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public void Initialize()
        {

        }

        private void DebugRendering()
        {
            int fieldWidth = _field.Cells.GetLength(0);
            int fieldHeight = _field.Cells.GetLength(1);

            for (int x = 0; x <= _field.Bounds.Width; x += TileMetrics.Width)
            {
                _spriteBatch.DrawLine(new Vector2(x, 0), new Vector2(x, _field.Bounds.Height), Color.Black);
            }

            for (int y = 0; y <= _field.Bounds.Height; y += TileMetrics.Height)
            {
                _spriteBatch.DrawLine(new Vector2(0, y), new Vector2(_field.Bounds.Width, y), Color.Black);
            }

            for (int x = 0; x < fieldWidth; x++)
            {
                for (int y = 0; y < fieldHeight; y++)
                {
                    var cell = _field.Cells[x, y];

                    _spriteBatch.DrawString(
                        spriteFont: _spriteFont,
                        text: $"{x};{y}",
                        position: cell.Position,
                        color: Color.White,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: 1f,
                        effects: SpriteEffects.None,
                        layerDepth: BackToFrontLayers.Debug);
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (_renderConfiguration.IsRenderingEnabled)
            {
                Rendering();
            }

            if (_renderConfiguration.IsDebugRenderingEnabled)
            {
                DebugRendering();
            }
        }

        private void Rendering()
        {
            int fieldWidth = _field.Cells.GetLength(0);
            int fieldHeight = _field.Cells.GetLength(1);

            for (int x = 0; x < fieldWidth; x++)
            {
                for (int y = 0; y < fieldHeight; y++)
                {
                    var cell = _field.Cells[x, y];

                    if (cell.CellType == CellType.Tree)
                    {
                        DrawGrassWithTree(cell);
                    }
                    else if (cell.CellType == CellType.Grass)
                    {
                        DrawGrass(cell);
                    }
                }
            }
        }

        private void DrawGrass(Cell cell)
        {
            _spriteBatch.Draw(
                texture: _grassTexture.Texture,
                destinationRectangle: cell.BoundsF.ToRectangle(),
                sourceRectangle: _grassTexture.Bounds,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                layerDepth: BackToFrontLayers.Grass);
        }

        private void DrawTree(Cell cell)
        {
            _spriteBatch.Draw(
                texture: _treeTexture.Texture,
                destinationRectangle: cell.BoundsF.ToRectangle(),
                sourceRectangle: _treeTexture.Bounds,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                layerDepth: BackToFrontLayers.Tree);
        }

        private void DrawGrassWithTree(Cell cell)
        {
            DrawGrass(cell);
            DrawTree(cell);
        }
    }
}
