using Microsoft.Xna.Framework;

namespace SnakeGame.Shared.SceneManagement
{
    public interface ISceneManager : IGameComponent, IDrawable, IUpdateable
    {
        IScene CurrentScene { get; }
        void Load(IScene scene);
    }
}