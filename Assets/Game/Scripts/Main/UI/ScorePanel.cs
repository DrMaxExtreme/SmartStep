using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private SpawnCountManager _spawnCountManager;

    [SerializeField] private TMP_Text _totalClearedObjectsText;
    [SerializeField] private TMP_Text _countSpawnText;
    [SerializeField] private TMP_Text _totalScoreText;
    [SerializeField] private TMP_Text _targetScoreText;

    private int _oldClearedObjectsText = 0;
    private int _oldTotalScore = 0;

    private const string ZeroText = "0";
    private const float TimeAnimation = 0.5f;

    public void Init(int targetScore, int spawnCount)
    {
        _totalClearedObjectsText.text = ZeroText;
        _totalScoreText.text = ZeroText;
        _targetScoreText.text = FormatWithSpaces(targetScore);
        _countSpawnText.text = Convert.ToString(spawnCount);
    }

    public string FormatWithSpaces(int number)
    {
        return number.ToString("N0", new System.Globalization.CultureInfo("ru-RU"));
    }

    private void UpdateTextSpawnCount(int spawnCount)
    {
        _countSpawnText.text = Convert.ToString(spawnCount);
    }

    private void UpdateScoreTexts(int totalScore, int totalClearedObjects)
    {
        StartCoroutine(AnimateInt(_oldTotalScore, totalScore, _totalScoreText));
        StartCoroutine(AnimateInt(_oldClearedObjectsText, totalClearedObjects, _totalClearedObjectsText));

        _oldClearedObjectsText = totalClearedObjects;
        _oldTotalScore = totalScore;
    }

    private IEnumerator AnimateInt(int startValue, int endValue, TMP_Text text)
    {
        float elapsedTime = 0f;
        int lastEmittedValue = startValue;

        text.text = startValue.ToString();

        while (elapsedTime < TimeAnimation)
        {
            elapsedTime += Time.deltaTime;

            float normalizedTime = Mathf.Clamp01(elapsedTime / TimeAnimation);

            int currentValue = Mathf.FloorToInt(Mathf.Lerp(startValue, endValue, normalizedTime));

            if (currentValue != lastEmittedValue)
            {
                lastEmittedValue = currentValue;
                text.text = FormatWithSpaces(lastEmittedValue).ToString();
            }

            yield return null;
        }

        if (lastEmittedValue != endValue)
            text.text = FormatWithSpaces(endValue).ToString();
    }

    private void OnEnable()
    {
        _scoreManager.ScoreChanged += UpdateScoreTexts;
        _spawnCountManager.Changed += UpdateTextSpawnCount;
        Init(1500, 4);//заглушка. init должен вызываться из LevelManager
        _scoreManager.Init(1500);
    }

    private void OnDisable()
    {
        _scoreManager.ScoreChanged -= UpdateScoreTexts;
        _spawnCountManager.Changed -= UpdateTextSpawnCount;
    }
}
