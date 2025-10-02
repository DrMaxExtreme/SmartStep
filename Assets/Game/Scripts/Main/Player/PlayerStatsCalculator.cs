using UnityEngine;

public class PlayerStatsCalculator
{
    [SerializeField] private PlayerData _playerData;

    public PlayerStatsCalculator()
    {

    }

    public int CalculateGold(int gold)
    {
        if (_playerData.Gold + gold < 0)
            return 0;

        return gold;
    }

    public int CalculateExperience(int experience)
    {
        if (experience < 0)
            return 0;

        return experience;
    }

    public int CalculateRank(int score, int targetScore)
    {

    }

    public int CalculateTargetScore(int targetScore, int rank, int referenceRank, int referenceTargetScore)
    {

    }
}
