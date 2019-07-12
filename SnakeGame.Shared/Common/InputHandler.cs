using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Shared.Common
{
    public class InputHandler : GameComponent
    {
        private static KeyboardState _keyboardState;
        private static KeyboardState _lastKeyboardState;

        public InputHandler(Game game) : base(game)
        {

        }

        public KeyboardState CurrentState => _keyboardState;

        public override void Initialize()
        {
            base.Initialize();

            _keyboardState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public static void Flush()
        {
            _lastKeyboardState = _keyboardState;
        }

        public static bool IsKeyReleased(Keys key)
        {
            return _keyboardState.IsKeyUp(key) && _lastKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return _keyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }
    }
}
