using UnityEngine;

public class PlayerStatsCalculator // Разделить обязанности наотдельные классы: для золота, для ранга и т.д.
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
}
