using UnityEngine;
using UnityEngine.UI;

public class ScoreField : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private Slider _firstScoreField;
    [SerializeField] private Slider _secondScoreField;

    private float _targetScore;

    private AnimationHelper _animator;

    [SerializeField] private float _durationAnimation = 0.266f;
    [SerializeField] private float _delayAnimation = 0.133f;

    private void Awake()
    {
        _animator = new AnimationHelper(_durationAnimation, _delayAnimation);
    }

    public void Init(int targetScore)
    {
        _targetScore = targetScore;
        _firstScoreField.value = 0f;
        _secondScoreField.value = 0f;
    }

    private void OnEnable()
    {
        _scoreManager.ScoreChanged += UpdateValue;
    }

    private void OnDisable()
    {
        _scoreManager.ScoreChanged -= UpdateValue;
    }

    private void UpdateValue(int totalScore, int totalCountClearedObjects)
    {
        float normalized = totalScore / _targetScore;

        _firstScoreField.value = normalized;

        StartCoroutine(_animator.Animate(_secondScoreField.value, normalized,
            value => _secondScoreField.value = value));
    }
}
