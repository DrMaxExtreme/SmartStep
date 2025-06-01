using System;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public event Action<int,int> ScoreChanged;

    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ScoreField _scoreFields;

    private int _totalClearedObjects = 0;
    private int _totalScore = 0;
    private int _targetScore;
    private int _baseScore = 10;
    private int _bonusPerCount = 1;
    private int _bonusScorePerStep = 5;

    public void Init(int targetScore)
    {
        _targetScore = targetScore;
        _scoreFields.Init(targetScore);
    }

    private void OnEnable()
    {
        _gridManager.ObjectsCleared += Calculate;
    }

    private void OnDisable()
    {
        _gridManager.ObjectsCleared -= Calculate;
    }

    private void Calculate(int clearedObjectsCount)
    {
        int additionalPoints = _bonusPerCount * (clearedObjectsCount * (clearedObjectsCount - 1) / 2);
        int getScore = clearedObjectsCount * _baseScore + additionalPoints + _bonusScorePerStep;

        _totalClearedObjects += clearedObjectsCount;
        _totalScore += getScore;

        ScoreChanged?.Invoke(_totalScore, _totalClearedObjects);
    }
}
