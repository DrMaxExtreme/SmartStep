using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameplaySimulation
{
    public class GameplaySimulationRunner
    {
        private int _simulationsCount;
        private readonly PlayfieldManager _playfieldManager;
        private readonly ScoreCalculator _scoreCalculator;
        private readonly RankManager _rankManager;
        private readonly SpawnManager _spawnManager;

        public GameplaySimulationRunner(int simulationsCount)
        {
            _simulationsCount = simulationsCount;
            _playfieldManager = new PlayfieldManager();
            _scoreCalculator = new ScoreCalculator();
            _rankManager = new RankManager();
            _spawnManager = new SpawnManager();
        }

        public void RunSimulations()
        {
            List<int> resultsScore = new List<int>(); //test
            int bestRunk = 0;

            for (int i = 0; i < _simulationsCount; i++)
            {
                _playfieldManager.CreateEmptyPlayfield();
                _playfieldManager.FillStartPlayfield();
                _rankManager.UpdateRankMultiplier();
                _rankManager.UpdateTarget();
                _spawnManager.AdjustSpawnConditions(_rankManager.Rank);
                _scoreCalculator.ClearScore();
                _playfieldManager.ClearTotalDestroyedObjects();

                Debug.Log($"Need destroyed for + spawn = {_spawnManager.GetCurrentSpawnThreshold()}");

                bool isGameOver = false;

                while (!isGameOver)
                {
                    var (row, column, numberDestroyedObjects) = _playfieldManager.FindBestMove();

                    _playfieldManager.MakeMove(row, column, numberDestroyedObjects);
                    _scoreCalculator.Calculate(_playfieldManager.TotalDestroyedObjects, numberDestroyedObjects);

                    if (_scoreCalculator.IsMaxScoreReached())
                    {
                        Debug.Log("Превышен лимит очков! Неправильный баланс!");
                        isGameOver = true;
                        break;
                    }

                    _playfieldManager.DropNumbersDown();
                    isGameOver = _playfieldManager.FillRandomNumbersInTopRow();
                    _playfieldManager.DropNumbersDown();
                }

                _rankManager.UpdateRank(_scoreCalculator.Score);
                MonoBehaviour.print($"New rank: {_rankManager.Rank}, get score = {_scoreCalculator.Score}");
                resultsScore.Add(_scoreCalculator.Score);
                
                if(bestRunk < _rankManager.Rank)
                {
                    bestRunk = _rankManager.Rank;
                }
            }

            Debug.Log($"Best rank: {bestRunk}");
            DisplaySimulationResults(resultsScore); //test
        }

        private void DisplaySimulationResults(List<int> results)
        {
            if (results.Count == 0)
            {
                MonoBehaviour.print("Нет результатов для отображения.");
                return;
            }

            int bestResult = results.Max();
            MonoBehaviour.print($"Лучший результат: {bestResult}");

            int averageResult = Mathf.RoundToInt((float)results.Average());
            MonoBehaviour.print($"Средний результат: {averageResult}");

            int worstResult = results.Min();
            MonoBehaviour.print($"Худший результат: {worstResult}");
        }
    }
}
