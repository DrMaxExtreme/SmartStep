using System.Diagnostics;

namespace GameplaySimulation
{
    public class ScoreCalculator
    {
        private const int MaxScore = 1000000;

        private const int StartCountDestroyToIncreaseSpawn = 200;

        private const int GetStartScoreWhenSpawnFour = 10;
        private const int IncreaseScoreWhenSpawnFour = 1;

        private const int GetStartScoreWhenSpawnFive = 12;
        private const int IncreaseScoreWhenSpawnFive = 2;

        private const int GetStartScoreWhenSpawnSix = 15;
        private const int IncreaseScoreWhenSpawnSix = 3;

        public int Score { get; private set; }

        public void Calculate(int totalDestroyedObjects, int numberDestroyedObjects)
        {
            int scoreForMove = 0;

            if (totalDestroyedObjects < StartCountDestroyToIncreaseSpawn)
            {
                for (int i = 0; i < numberDestroyedObjects; i++)
                {
                    scoreForMove += GetStartScoreWhenSpawnFour + IncreaseScoreWhenSpawnFour * i;
                }
            }
            else if (totalDestroyedObjects >= StartCountDestroyToIncreaseSpawn && totalDestroyedObjects < StartCountDestroyToIncreaseSpawn + StartCountDestroyToIncreaseSpawn)
            {
                for (int i = 0; i < numberDestroyedObjects; i++)
                {
                    scoreForMove += GetStartScoreWhenSpawnFive + IncreaseScoreWhenSpawnFive * i;
                }
            }
            else if (totalDestroyedObjects >= StartCountDestroyToIncreaseSpawn + StartCountDestroyToIncreaseSpawn)
            {
                for (int i = 0; i < numberDestroyedObjects; i++)
                {
                    scoreForMove += GetStartScoreWhenSpawnSix + IncreaseScoreWhenSpawnSix * i;
                }
            }
            else
            {
                scoreForMove = 0;
            }

            Score += scoreForMove;
        }

        public void ClearScore()
        {
            Score = 0;
        }

        public bool IsMaxScoreReached()
        {
            return Score >= MaxScore;
        }
    }
}
