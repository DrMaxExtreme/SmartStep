using System;
using System.Collections;
using UnityEngine;

public class AnimationHelper
{
    private readonly float _duration;
    private readonly float _delay;

    public AnimationHelper(float duration, float delay)
    {
        _duration = duration;
        _delay = delay;
    }

    public IEnumerator Animate(float startValue, float targetValue, Action<float> onValueChanged)
    {
        yield return new WaitForSeconds(_delay);

        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _duration);
            float current = Mathf.Lerp(startValue, targetValue, t);
            onValueChanged(current);
            yield return null;
        }

        onValueChanged(targetValue);
    }
}
