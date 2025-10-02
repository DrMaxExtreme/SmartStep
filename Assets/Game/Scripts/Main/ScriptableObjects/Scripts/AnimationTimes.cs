using UnityEngine;

[CreateAssetMenu(menuName = "Game/AnimationTimes", fileName = "AnimationTimes")]
public class AnimationTimes : ScriptableObject
{
    [SerializeField] private float _duration = 0.266f;
    [SerializeField] private float _delay = 0.133f;

    public float Duration => _duration;
    public float Delay => _delay;
}
