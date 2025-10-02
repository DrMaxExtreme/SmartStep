using System;
using System.Collections;
using UnityEngine;

public class AnimationHelper
{
    [SerializeField] private AnimationTimes _animationTimes;

    public IEnumerator Animate(float startValue, float targetValue, Action<float> onValueChanged)
    {
        yield return new WaitForSeconds(_animationTimes.Delay);

        float elapsed = 0f;
        while (elapsed < _animationTimes.Duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _animationTimes.Duration);
            float current = Mathf.Lerp(startValue, targetValue, t);
            onValueChanged(current);
            yield return null;
        }

        onValueChanged(targetValue);
    }
}
