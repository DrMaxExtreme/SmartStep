using MPUIKIT;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreField : MonoBehaviour
{
    [SerializeField] private ScoreCounter _scoreCounter;

    [SerializeField] private Slider _firstScoreField;
    [SerializeField] private Slider _secondScoreField;

    private float _targeScore;

    private float _durationAnimation = 0.266f;
    private float _delayAnimation = 0.133f;

    public void Init(int targetScore)
    {
        _targeScore = targetScore;
    }

    public void UpdateValue(int totalScore, int totalCountClearedObjects)
    {
        float normilizedTotalScore = (float)totalScore / _targeScore;

        _firstScoreField.value = normilizedTotalScore;
        StartCoroutine(AnimateValue(normilizedTotalScore, _secondScoreField));
    }

    private IEnumerator AnimateValue(float targetValue, Slider fill)
    {
        yield return new WaitForSeconds(_delayAnimation);

        float startValue = fill.value;
        float elapsedTime = 0f;

        while (elapsedTime < _durationAnimation)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / _durationAnimation);
            fill.value = Mathf.Lerp(startValue, targetValue, normalizedTime);
            yield return null;
        }

        fill.value = targetValue;
    }

    private void OnEnable()
    {
        _scoreCounter.ScoreChanged += UpdateValue;
        _firstScoreField.value = 0;
        _secondScoreField.value = 0;
    }

    private void OnDisable()
    {
        _scoreCounter.ScoreChanged -= UpdateValue;
    }
}
