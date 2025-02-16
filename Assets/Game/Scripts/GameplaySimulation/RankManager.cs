using System;
using UnityEngine;

namespace GameplaySimulation
{
    public class RankManager
    {
        private const int StartTargetScore = 2500;
        private const float StartMultiplier = 0.25f;
        private const float ChangeRankInterval = 10000;
        private const float ChangeTargetRankInterval = 15000;
        private const int MinRankChange = 250;
        private const int MaxRankChange = 1000;

        private float _multiplier = StartMultiplier;
        private int _targetScore = StartTargetScore;

        public int Rank { get; private set; } = 0;

        public void UpdateRankMultiplier()
        {
            float currentRank = Rank;

            _multiplier = StartMultiplier + currentRank / ChangeRankInterval * StartMultiplier;
        }

        public void UpdateTarget()
        {
            float currentRank = Rank;

            _targetScore = Convert.ToInt32(Mathf.Round(
                StartTargetScore + currentRank / ChangeTargetRankInterval * StartTargetScore));
        }

        public void UpdateRank(int score)
        {
            int scoreDifference = score - _targetScore;
            MonoBehaviour.print($"Target = {_targetScore}");

            int rankChange = Mathf.RoundToInt((scoreDifference >= 0 ? MinRankChange : -MinRankChange) + scoreDifference * _multiplier);

            if (rankChange > 0)
            {
                Debug.Log("Победа");
                rankChange = Mathf.Min(rankChange, MaxRankChange);
            }
            else
            {
                Debug.Log("Проигрыш");
                rankChange = Mathf.Max(rankChange, -MaxRankChange);
            }

            if (Rank + rankChange > 0)
            {
                Rank += rankChange;
            }
            else
            {
                Rank = 0;
            }
        }
    }
}
