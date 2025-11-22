using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/AnimationTimes", fileName = "GamesStatisticData")]
public class GamesStatisticData : ScriptableObject
{
    [SerializeField] private const int MaxGameResultsCount = 20;

    [SerializeField] private int _highestScore = 0;
    [SerializeField] private int _highestRank = 0;

    [SerializeField] private int _totalGames = 0;

    [SerializeField] private float _averageStepsPerGame = 0;
    [SerializeField] private float _averageCollectPerStep = 0;
    [SerializeField] private float _averageCollectPerGame = 0;

    [Serializable]
    private class GameResult
    {
        public int Steps;
        public int Collected;
    }

    [SerializeField] private List<GameResult> _lastGames = new List<GameResult>();

    public int HighestScore => _highestScore; // добавить метод подписаный на событие rankChange
    public int HighestRank => _highestRank;
    public int TotalGames => _totalGames;
    public float AverageStepsPerGame => _averageStepsPerGame;
    public float AverageCollectPerStep => _averageCollectPerStep;
    public float AverageCollectPerGame => _averageCollectPerGame;

    public void AddGameResult(int steps, int collected) // подписать этот метод на событие endGame
    {
        if (_lastGames.Count >= MaxGameResultsCount)
        {
            _lastGames.RemoveAt(0); // удаляем самый старый результат игры
        }

        _lastGames.Add(new GameResult { Steps = steps, Collected = collected });
        _totalGames++;
        RecalculateAverages();
    }

    private void RecalculateAverages()
    {
        if (_lastGames.Count == 0)
        {
            _averageStepsPerGame = 0;
            _averageCollectPerStep = 0;
            _averageCollectPerGame = 0;
            return;
        }

        int totalSteps = 0;
        int totalCollected = 0;

        foreach (var game in _lastGames)
        {
            totalSteps += game.Steps;
            totalCollected += game.Collected;
        }

        _averageStepsPerGame = (float)totalSteps / _lastGames.Count;
        _averageCollectPerGame = (float)totalCollected / _lastGames.Count;
        _averageCollectPerStep = totalSteps > 0 ? (float)totalCollected / totalSteps : 0;
    }
}
