using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event Action<int, int> ScoreChanged;

    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ScoreField _scoreField;

    [SerializeField] private int _baseScore = 10;
    [SerializeField] private int _bonusPerCount = 0;
    [SerializeField] private int _bonusScorePerStep = 0;

    private ScoreCalculator _calculator;

    public void Init(int targetScore)
    {
        _calculator = new ScoreCalculator(_baseScore, _bonusPerCount, _bonusScorePerStep);
        _scoreField.Init(targetScore);
    }

    private void OnEnable()
    {
        _gridManager.ObjectsCleared += OnObjectsCleared;
    }

    private void OnDisable()
    {
        _gridManager.ObjectsCleared -= OnObjectsCleared;
    }

    private void OnObjectsCleared(int count)
    {
        var (totalScore, totalCleared) = _calculator.Calculate(count);
        ScoreChanged?.Invoke(totalScore, totalCleared);
    }

    public void ResetScore()
    {
        _calculator.Reset();
        ScoreChanged?.Invoke(0, 0);
    }
}
