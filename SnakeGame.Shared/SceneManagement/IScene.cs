using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

namespace SnakeGame.Shared.SceneManagement
{
    public interface IScene
    {
        /// <summary>
        /// Should be invoked after constructor for loading resources, etc.
        /// </summary>
        void Load();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void Unload();
    }
}
