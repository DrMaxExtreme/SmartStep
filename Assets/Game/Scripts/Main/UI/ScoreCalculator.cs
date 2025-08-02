public class ScoreCalculator
{
    private readonly int _baseScore;
    private readonly int _bonusPerCount;
    private readonly int _bonusScorePerStep;

    private int _totalClearedObjects;
    private int _totalScore;

    public ScoreCalculator(int baseScore, int bonusPerCount, int bonusScorePerStep)
    {
        _baseScore = baseScore;
        _bonusPerCount = bonusPerCount;
        _bonusScorePerStep = bonusScorePerStep;
    }

    public (int totalScore, int totalCleared) Calculate(int clearedObjectsCount)
    {
        int additionalPoints = _bonusPerCount * (clearedObjectsCount * (clearedObjectsCount - 1) / 2);
        int getScore = clearedObjectsCount * _baseScore + additionalPoints + _bonusScorePerStep;

        _totalClearedObjects += clearedObjectsCount;
        _totalScore += getScore;

        return (_totalScore, _totalClearedObjects);
    }

    public void Reset()
    {
        _totalClearedObjects = 0;
        _totalScore = 0;
    }
}
