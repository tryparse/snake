using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.Common.ResourceManagers;
using SnakeGame.Shared.GameLogic.Food.Interfaces;
using SnakeGame.Shared.Graphics;
using SnakeGame.Shared.Renderers;
using System;

namespace SnakeGame.Shared.GameLogic.Food
{
    public class FoodGraphicsComponent : IGraphics2DComponent
    {
        private readonly IFood _food;
        private readonly IRenderingSystem _renderingCore;

        public FoodGraphicsComponent(IFood food, IRenderingSystem renderingCore)
        {
            _food = food ?? throw new ArgumentNullException(nameof(food));
            _renderingCore = renderingCore ?? throw new ArgumentNullException(nameof(renderingCore));
        }

        public void Draw(GameTime gameTime)
        {
            if (_renderingCore.RenderSettings.IsRenderingEnabled)
            {
                Rendering();
            }

            if (_renderingCore.RenderSettings.IsDebugRenderingEnabled)
            {
                DebugRendering();
            }
        }

        private void DebugRendering()
        {
            _renderingCore.SpriteBatch.DrawRectangle(_food.Bounds, Color.Orange, 1);
            _renderingCore.SpriteBatch.DrawString(_renderingCore.SpriteFont, _food.Position.ToString(), _food.Position, Color.Black);
        }

        private void Rendering()
        {
            var t = _renderingCore.TextureManager.TextureRegions["Fruit"];

            var origin = new Vector2(t.Width / 2, t.Height / 2);
            var scale = new Vector2(_food.Size.Width / t.Bounds.Width, _food.Size.Height / t.Bounds.Height);

            _renderingCore.SpriteBatch.Draw(
                    texture: t.Texture,
                    position: _food.Position,
                    sourceRectangle: t.Bounds,
                    color: Color.White,
                    rotation: _food.Rotation,
                    scale: scale,
                    origin: origin,
                    effects: SpriteEffects.None,
                    layerDepth: LayerDepths.Fruit
                );
        }
    }
}
