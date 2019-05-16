using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.UIComponents
{
    class KeyboardKeysComponent : DrawableGameComponent
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;
        private readonly Vector2 _position;
        private readonly Queue<Keys> _keys;
        private Texture2D _texture;

        public KeyboardKeysComponent(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Vector2 position, Queue<Keys> keys) : base(game)
        {
            this._spriteBatch = spriteBatch;
            this._spriteFont = spriteFont;
            this._position = position;
            this._keys = keys;

            CreateTexture();
        }

        private void CreateTexture()
        {
            var size = 100;

            var pixels = new Color[size * size];

            _texture = new Texture2D(_spriteBatch.GraphicsDevice, size, size);
            _texture.GetData(pixels);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var pixel = new Color(Color.DarkGray, 1f);
                    pixels[size * i + j] = pixel;
                }
            }

            _texture.SetData(pixels);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, _position, Color.White);
            _spriteBatch.DrawString(_spriteFont, "[Sample text]", _position, Color.White);
        }
    }
}
