using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.UIComponents
{
    public class PointsCounterComponent : DrawableGameComponent
    {
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly IGamePoints _gamePoints;
        private readonly Vector2 _position;
        private readonly TextureRegion2D _foodTexture2D;

        public PointsCounterComponent(Game game, Vector2 position, IGraphicsSystem graphicsSystem, IGamePoints gamePoints) : base(game)
        {
            _position = position;
            _graphicsSystem = graphicsSystem ?? throw new ArgumentNullException(nameof(graphicsSystem));
            this._gamePoints = gamePoints;

            _foodTexture2D = graphicsSystem.TextureManager.TextureRegions.GetRegion("Fruit");
        }

        public override void Draw(GameTime gameTime)
        {
            var size = new Size2(25, 25);
            var scale = new Vector2(size.Width / (float)_foodTexture2D.Width, size.Height / (float)_foodTexture2D.Height);
            var textPosition = new Vector2(_position.X + size.Width, _position.Y);

            _graphicsSystem.SpriteBatch.Draw(_foodTexture2D.Texture, _position, _foodTexture2D.Bounds, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, LayerDepths.UI);
            _graphicsSystem.SpriteBatch.DrawString(_graphicsSystem.SpriteFont, $" x {_gamePoints.Points}", textPosition, Color.Green);
        }
    }
}
