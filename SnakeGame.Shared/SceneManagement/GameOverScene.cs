using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Shared.SceneManagement
{
    class GameOverScene : IScene
    {
        private readonly SpriteBatch _spriteBatch;

        public GameOverScene(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
