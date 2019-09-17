using System;

namespace SnakeGame.Shared.GameLogic
{
    public class GamePoints : IGamePoints
    {
        private readonly int _startRemainingLives;

        public int Points { get; private set; }

        public int MaxPoints { get; private set; }

        public int RemainingLives { get; private set; }

        public GamePoints(int startRemainingLives)
        {
            _startRemainingLives = startRemainingLives;
            RemainingLives = startRemainingLives;
        }

        public void IncrementPoints()
        {
            Points++;
        }

        public void DecrementLives()
        {
            RemainingLives--;
        }

        public void Reset()
        {
            MaxPoints = Math.Max(MaxPoints, Points);
            Points = 0;
        }
    }
}
