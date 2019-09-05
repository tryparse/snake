using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic;
using SnakeGame.Shared.Graphics;

namespace SnakeGame.Shared.UIComponents
{
    public class RemainingLivesComponent : DrawableGameComponent
    {
        private readonly IGraphicsSystem _graphicsSystem;
        private readonly IGamePoints _gamePoints;
        private readonly Vector2 _position;
        private readonly TextureRegion2D _headTexture2D;

        public RemainingLivesComponent(Game game, Vector2 position, IGraphicsSystem graphicsSystem, IGamePoints gamePoints) : base(game)
        {
            _position = position;
            _graphicsSystem = graphicsSystem;
            _gamePoints = gamePoints;

            _headTexture2D = graphicsSystem.TextureManager.TextureRegions.GetRegion("Head");
        }

        public override void Draw(GameTime gameTime)
        {
            var size = new Size2(25, 25);
            var scale = new Vector2(size.Width / (float) _headTexture2D.Width,
                size.Height / (float) _headTexture2D.Height);
            var textPosition = new Vector2(_position.X + size.Width, _position.Y);

            _graphicsSystem.SpriteBatch.Begin(samplerState: SamplerState.PointClamp,
                sortMode: SpriteSortMode.BackToFront, blendState: BlendState.AlphaBlend,
                transformMatrix: _graphicsSystem.Camera2D.GetViewMatrix());

            _graphicsSystem.SpriteBatch.Draw(_headTexture2D.Texture, _position, _headTexture2D.Bounds, Color.White, 0,
                Vector2.Zero, scale, SpriteEffects.None, LayerDepths.UI);
            _graphicsSystem.SpriteBatch.DrawString(_graphicsSystem.SpriteFont, $" x {_gamePoints.RemainingLives}",
                textPosition, _gamePoints.RemainingLives < 2 ? Color.Red : Color.Green);

            _graphicsSystem.SpriteBatch.End();
        }
    }
}
