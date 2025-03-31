using System;
using UnityEngine;

public class WeaponEvents : MonoBehaviour
{
    public static event Action OnReload;

    public static void Reload()
    {
        OnReload?.Invoke();
    }

    public static event Action<float> OnAimingModeSpreadChanged;

    public static void ChangeAimingModeSpread(float spread)
    {
        OnAimingModeSpreadChanged?.Invoke(spread);
    }
}
