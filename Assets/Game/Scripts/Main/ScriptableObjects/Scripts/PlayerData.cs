using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player Data", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private string _username;

    [SerializeField] private int _level = 1;
    [SerializeField] private int _currentExperience = 0;
    //[SerializeField] private int _targetExperience = 100;
    //[SerializeField] private int _increaseTargetExperience = 100;

    [SerializeField] private int _gold = 0;
    [SerializeField] private int _rank = 0;
    [SerializeField] private int _targetScore = 0;

    public string Username => _username;
    public int Level => _level;
    public int Gold => _gold;
    public int Rank => _rank;

    /*
    public void ChangeGold(int gold)
    {
        _gold += _playerStatsCalculator.CalculateGold(gold);
    }

    public void AddExperience(int experience)
    {
        _currentExperience += _playerStatsCalculator.CalculateExperience(experience);

        if (_currentExperience >= _targetExperience)
        {
            _currentExperience -= _targetExperience;
            _targetExperience += _increaseTargetExperience;
            _level++;
        }
    }
    */

    /*
    public void ChangeRank(int scoreResult)
    {
        RankCalculator rankCalculator = new RankCalculator();
        TargetScoreCalculator targetScoreCalculator = new TargetScoreCalculator();

        _rank += rankCalculator.CalculateRankChange(scoreResult, _targetScore, _rank);
        _targetScore += targetScoreCalculator.CalculateTargetScore(_rank);
    }
    */
}
