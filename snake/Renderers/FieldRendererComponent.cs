using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.TextureAtlases;
using snake.GameEntities;
using snake.Interfaces;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField;
using SnakeGame.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.Renderers
{
    class FieldRendererComponent : IRenderer2D
    {
        private readonly Field _field;
        private readonly TextureAtlas _textureAtlas;
        private readonly SpriteFont _spriteFont;
        private readonly IGameSettings _gameSettings;
        private readonly SpriteBatch _spriteBatch;

        private int _drawOrder;
        private bool _isVisible;

        private TextureRegion2D _treeTexture;
        private TextureRegion2D _grassTexture;

        public FieldRendererComponent(IGameSettings gameSettings, SpriteBatch spriteBatch, SpriteFont spriteFont, IRenderSettings renderSettings, Field field, TextureAtlas textureAtlas, int drawOrder = 0)
        {
            this._gameSettings = gameSettings;
            this._spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            this._spriteFont = spriteFont ?? throw new ArgumentNullException(nameof(spriteFont));
            this.RenderSettings = renderSettings ?? throw new ArgumentNullException(nameof(renderSettings));
            this._field = field ?? throw new ArgumentNullException(nameof(field));
            this._textureAtlas = textureAtlas ?? throw new ArgumentNullException(nameof(textureAtlas));
            this._drawOrder = drawOrder;

            Initialize();
        }

        public IRenderSettings RenderSettings { get; }

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
            this._isVisible = true;

            ReadTextureRegions();
        }

        private void ReadTextureRegions()
        {
            this._treeTexture = _textureAtlas.GetRegion("Tree");
            this._grassTexture = _textureAtlas.GetRegion("Grass");
        }

        private void DebugRendering()
        {
            RenderTileBorders();

            int fieldWidth = _field.Cells.GetLength(0);
            int fieldHeight = _field.Cells.GetLength(1);

            for (int x = 0; x < fieldWidth; x++)
            {
                for (int y = 0; y < fieldHeight; y++)
                {
                    var cell = _field.Cells[x, y];

                    RenderCellIndices(cell);
                }
            }
        }

        private void RenderCellIndices(Cell cell)
        {
            _spriteBatch.DrawString(
                spriteFont: _spriteFont,
                text: $"{cell.Indices.X};{cell.Indices.Y}",
                position: cell.Position,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 1f,
                effects: SpriteEffects.None,
                layerDepth: LayerDepths.Debug);
        }

        private void RenderTileBorders()
        {
            for (int x = 0; x <= _field.Bounds.Width; x += _gameSettings.TileWidth)
            {
                _spriteBatch.DrawLine(new Vector2(x, 0), new Vector2(x, _field.Bounds.Height), Color.Black);
            }

            for (int y = 0; y <= _field.Bounds.Height; y += _gameSettings.TileHeight)
            {
                _spriteBatch.DrawLine(new Vector2(0, y), new Vector2(_field.Bounds.Width, y), Color.Black);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (RenderSettings.IsRenderingEnabled)
            {
                Rendering();
            }

            if (RenderSettings.IsDebugRenderingEnabled)
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
                layerDepth: LayerDepths.Grass);
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
                layerDepth: LayerDepths.Tree);
        }

        private void DrawGrassWithTree(Cell cell)
        {
            DrawGrass(cell);
            DrawTree(cell);
        }
    }
}
