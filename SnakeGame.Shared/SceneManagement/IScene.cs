using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SnakeGame.Shared.SceneManagement
{
    interface IScene
    {
        void Initialize();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }

    interface ISceneManager
    {
        IScene CurrentScene { get; }

        void Load(IScene scene);
    }

    class SceneManager : ISceneManager
    {
        public IScene CurrentScene { get; private set; }

        public void Load(IScene scene)
        {
            throw new NotImplementedException();
        }
    }
}
