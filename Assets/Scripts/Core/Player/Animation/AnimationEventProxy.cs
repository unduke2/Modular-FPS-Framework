using System;
using UnityEngine;

public class AnimationEventProxy : MonoBehaviour
{
    private AnimationComponent _animationComponent;

    private void Start()
    {
        _animationComponent = GetComponentInChildren<AnimationComponent>();

        if (_animationComponent == null)
        {
            Debug.LogWarning($"{nameof(AnimationEventProxy)}: No AnimationComponent found");
        }
    }

    public void ReloadComplete()
    {
        if (_animationComponent != null)
        {
            _animationComponent.OnReloadAnimationComplete();
        }
        else
        {
            Debug.LogWarning($"{nameof(AnimationEventProxy)}: No AnimationComponent reference found - cannot call OnReloadAnimationComplete");
        }
    }
}
