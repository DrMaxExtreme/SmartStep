using MPUIKIT;
using System;
using System.Collections;
using UnityEngine;

public class SpawnCountManager : MonoBehaviour
{
    public event Action<int> Changed;

    [SerializeField] private GridManager _gridManager;

    [SerializeField] private int _stepsForLevelUp = 25;
    [SerializeField] private int _startCountSpawn = 4;
    [SerializeField] private int _maxSpawnCount = 6;

    [SerializeField] private MPImage _fillFirst;
    [SerializeField] private MPImage _fillSecond;

    private float _durationAnimation = 0.266f;
    private float _delayAnimation = 0.133f;

    private int _doneStepsCount;
    private int _spawnCount;

    public int StartCountSpawn => _startCountSpawn;

    public void TryChange()
    {
        if (_spawnCount == _maxSpawnCount)
        {
            return;
        }

        _doneStepsCount++;

        if (_doneStepsCount == _stepsForLevelUp)
        {
            StartCoroutine(HandleLevelUp());
        }
        else
        {
            StartCoroutine(AnimateFill(false));
        }
    }
    private IEnumerator HandleLevelUp()
    {
        yield return AnimateFill(true);

        _doneStepsCount = 0;
        _spawnCount++;

        Changed?.Invoke(_spawnCount);

        StartCoroutine(AnimateFill(false));
    }
    private IEnumerator AnimateFill(bool isLastStep)
    {
        float startValue = ((float)_doneStepsCount - 1) / _stepsForLevelUp;
        float endValue = (float)_doneStepsCount / _stepsForLevelUp;
        float elapsed = 0f;

        _fillFirst.fillAmount = endValue;

        yield return new WaitForSeconds(_delayAnimation);

        while (elapsed < _durationAnimation)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsed / _durationAnimation);
            _fillSecond.fillAmount = Mathf.Lerp(startValue, endValue, normalizedTime);
            yield return null;
        }

        _fillSecond.fillAmount = endValue;
    }

    private void OnEnable()
    {
        _fillFirst.fillAmount = 0;
        _fillSecond.fillAmount = 0;
        _doneStepsCount = 0;
        _spawnCount = _startCountSpawn;

        _gridManager.DidStep += TryChange;
    }

    private void OnDisable()
    {
        _gridManager.DidStep -= TryChange;
    }
}
