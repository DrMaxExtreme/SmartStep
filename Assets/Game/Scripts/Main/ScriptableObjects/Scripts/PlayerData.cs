using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using YG.Utils.LB;

[CreateAssetMenu(menuName = "Game/Player Data", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    private const int MaxGameResultsCount = 20;
    private const int ReferenceRank = 30000;
    private const int ReferenceTargetScore = 10000;
    private const int MinRankChange = 250;

    [SerializeField] private string _username;

    [SerializeField] private int _level = 1;
    [SerializeField] private int _currentExperience = 0;
    [SerializeField] private int _targetExperience = 100;
    [SerializeField] private int _increaseTargetExperience = 100;

    [SerializeField] private int _gold = 0;

    [SerializeField] private int _rank = 0;
    [SerializeField] private int _highestRank = 0;

    [SerializeField] private int _targetScore = 1000;
    [SerializeField] private int _highestScore = 0;

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

    private PlayerStatsCalculator _playerStatsCalculator = new PlayerStatsCalculator();

    // === Public getters ===
    public string Username => _username;
    public int Level => _level;
    public int Gold => _gold;
    public int Rank => _rank;
    public int HighestRank => _highestRank;
    public int HighestScore => _highestScore;
    public int TotalGames => _totalGames;

    public float AverageStepsPerGame => _averageStepsPerGame;
    public float AverageCollectPerStep => _averageCollectPerStep;
    public float AverageCollectPerGame => _averageCollectPerGame

    // === Add new game result ===
    public void AddGameResult(int steps, int collected)
    {
        if (_lastGames.Count >= MaxGameResultsCount)
        {
            _lastGames.RemoveAt(0); // удаляем самую старую игру
        }

        _lastGames.Add(new GameResult { Steps = steps, Collected = collected });
        _totalGames++;
        RecalculateAverages();
    }

    // === Пересчёт средних значений ===
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

    // === Сохранение / Загрузка ===
    [Serializable]
    private class SaveWrapper
    {
        public string username;
        public int level, currentExp, targetExp, increaseTargetExp;
        public int gold;
        public int rank, highestRank;
        public int highestScore, totalGames;
        public List<GameResult> lastGames;
    }

    public void Save()
    {
        SaveWrapper wrapper = new SaveWrapper
        {
            username = _username,
            level = _level,
            currentExp = _currentExperience,
            targetExp = _targetExperience,
            increaseTargetExp = _increaseTargetExperience,
            gold = _gold,
            rank = _rank,
            highestRank = _highestRank,
            highestScore = _highestScore,
            totalGames = _totalGames,
            lastGames = _lastGames
        };

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString("player_data", json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey("player_data")) return;

        string json = PlayerPrefs.GetString("player_data");
        SaveWrapper wrapper = JsonUtility.FromJson<SaveWrapper>(json);

        _username = wrapper.username;
        _level = wrapper.level;
        _currentExperience = wrapper.currentExp;
        _targetExperience = wrapper.targetExp;
        _increaseTargetExperience = wrapper.increaseTargetExp;
        _gold = wrapper.gold;
        _rank = wrapper.rank;
        _highestRank = wrapper.highestRank;
        _highestScore = wrapper.highestScore;
        _totalGames = wrapper.totalGames;
        _lastGames = wrapper.lastGames ?? new List<GameResult>();

        RecalculateAverages();
    }

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

    public void ChangeRank(int score)
    {
        _rank += _playerStatsCalculator.CalculateRank(score, _targetScore);
        _targetScore += _playerStatsCalculator.CalculateTargetScore(_targetScore, _rank, ReferenceRank, ReferenceTargetScore);
    }
}
