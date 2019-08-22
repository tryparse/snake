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
            CurrentScene = scene;
            scene.Initialize();
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