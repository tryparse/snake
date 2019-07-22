namespace SnakeGame.Shared.GameLogic
{
    public class GamePoints : IGamePoints
    {
        public int Points { get; private set; }

        public void IncrementPoints()
        {
            Points++;
        }

        public void Reset()
        {
            Points = 0;
        }
    }
}
