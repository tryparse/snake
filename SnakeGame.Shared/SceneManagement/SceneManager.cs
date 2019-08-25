using Microsoft.Xna.Framework;

namespace SnakeGame.Shared.SceneManagement
{
    public class SceneManager : DrawableGameComponent, ISceneManager
    {
        public SceneManager(Game game) : base(game)
        {
        }

        public IScene CurrentScene { get; private set; }

        public void Load(IScene scene)
        {
            CurrentScene?.Unload();

            CurrentScene = scene;
            CurrentScene.Load();
        }

        public override void Update(GameTime gameTime)
        {
            CurrentScene?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            CurrentScene?.Draw(gameTime);
        }
    }
}