using UnityEngine;

public class RankCalculator
{
    [Header("Rank Settings")]
    [SerializeField] private int _baseChange = 30;
    [SerializeField] private int _growthFactor = 10;
    [SerializeField] private int _minRank = 0;

    [Header("Special Rules")]
    [SerializeField] private int _zeroDeltaRankBonus = 250;

    public int CalculateRankChange(int scoreResult, int targetScore, int currentRank)
    {
        float deltaRank = CalculateRankDelta(scoreResult, targetScore, currentRank);

        if (deltaRank == 0)
            return _zeroDeltaRankBonus;

        bool rankIncreases = deltaRank > 0;
        float absDeltaRank = Mathf.Abs(deltaRank);

        int delta = Mathf.RoundToInt((_baseChange + (currentRank / _growthFactor)) * absDeltaRank);

        if (!rankIncreases)
            delta = -delta;

        return delta;
    }

    private int CalculateRankDelta(int scoreResult, int targetScore, int currentRank)
    {
        float coefficient = (float)(scoreResult - targetScore) / targetScore;

        float normalizedProgress = Mathf.Clamp01(currentRank / 30000f);

        float kMin = Mathf.Max(-0.5f + (0.3f * normalizedProgress), -0.1f);
        float kMax = Mathf.Max(1.0f - (0.75f * normalizedProgress), 0.1f);

        float deltaRank;

        if (coefficient < 0f)
        {
            if (coefficient <= kMin)
                deltaRank = -500;
            else
            {
                float factor = Mathf.InverseLerp(0f, kMin, coefficient);
                deltaRank = Mathf.Lerp(-250f, -500f, factor);
            }
        }
        else if (coefficient > 0f)
        {
            if (coefficient >= kMax)
                deltaRank = 500;
            else
            {
                float factor = Mathf.InverseLerp(0f, kMax, coefficient);
                deltaRank = Mathf.Lerp(250f, 500f, factor);
            }
        }
        else
        {
            deltaRank = 250f; // ровно на границе
        }

        return Mathf.RoundToInt(deltaRank);
    }
}
