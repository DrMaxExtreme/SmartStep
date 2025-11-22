using UnityEngine;

public class TargetScoreCalculator : MonoBehaviour
{
    [SerializeField] private int _baseScore = 1000;
    [SerializeField] private float _slope = 0.3f;

    public int CalculateTargetScore(int rank)
    {
        if (rank < 0)
            rank = 0;

        float value = _baseScore + rank * _slope;

        return Mathf.RoundToInt(value);
    }
}

