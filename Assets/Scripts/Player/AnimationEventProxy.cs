using System;
using UnityEngine;

public class AnimationEventProxy : MonoBehaviour
{
    public static event Action OnReloadComplete;
    public void ReloadComplete()
    {
        Debug.Log("Animation Event Triggered: Reload Complete");
        OnReloadComplete?.Invoke();
    }
}
