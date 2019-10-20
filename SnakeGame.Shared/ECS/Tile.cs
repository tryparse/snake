using MonoGame.Extended.Sprites;

namespace SnakeGame.Shared.ECS
{
    class Tile
    {
        public Sprite Sprite { get; private set; }

        public Tile(Sprite sprite)
        {
            Sprite = sprite;
        }
    }
}
