using System;
using UnityEngine;

public static class EventManager
{
    // Declare the events
    public static event Action<Vector3, float> OnUpdateWeaponPosition;
    public static event Action<Vector3, float> OnUpdateWeaponRotation;
    public static event Action<float> OnUpdateWeaponSensitivity;
    public static event Action<float, float> OnUpdateCamera;
    public static event Action<AimingMode> OnUpdateAimingMode;
    public static event Action<float> OnMovementSpeedUpdate;
    public static event Action OnFlashlightPressed;
    public static event Action<Vector2> OnCameraNoiseUpdate;
    public static event Action<Quaternion> OnCameraRotationUpdate;







    public static event Action<PlayerStance> OnPlayerStanceUpdate;

    public static void TriggerPlayerStance(PlayerStance playerStance)
    {
        OnPlayerStanceUpdate?.Invoke(playerStance);
    }

    // Method to invoke OnUpdateWeaponTransform
    public static void TriggerWeaponPosition(Vector3 positionOffset, float transitionSpeed)
    {
        OnUpdateWeaponPosition?.Invoke(positionOffset, transitionSpeed);
    }
    public static void TriggerWeaponRotation(Vector3 rotationOffset, float transitionSpeed)
    {
        OnUpdateWeaponRotation?.Invoke(rotationOffset, transitionSpeed);
    }
    public static void TriggerWeaponSensitivity(float sensitivity)
    {
        OnUpdateWeaponSensitivity?.Invoke(sensitivity);
    }
    public static void TriggerWeaponTransform(float sensitivityMultiplier)
    {
        OnUpdateWeaponSensitivity?.Invoke(sensitivityMultiplier);
    }

    // Method to invoke OnUpdateCamera
    public static void TriggerCameraUpdate(float fieldOfView, float transitionSpeed)
    {
        Debug.Log("Camera Update Triggered");
        OnUpdateCamera?.Invoke(fieldOfView, transitionSpeed);
    }

    public static void TriggerAimingModeUpdate(AimingMode mode)
    {
        //Debug.Log("Aiming Mode Update Triggered");
        OnUpdateAimingMode?.Invoke(mode);
    }

    public static void TriggerCameraNoiseUpdate(Vector2 noise)
    {
        //Debug.Log("Camera Noise Update Triggered");
        OnCameraNoiseUpdate?.Invoke(noise);
    }


    public static void TriggerMovementSpeedUpdate(float speedMultiplier)
    {
        Debug.Log("Movement Speed Update Triggered");
        OnMovementSpeedUpdate?.Invoke(speedMultiplier);
    }

    public static void TriggerFlashlightPressed()
    {
        Debug.Log("Flashlight triggered");
        OnFlashlightPressed?.Invoke();
    }

    public static void TriggerCameraRotationUpdate(Quaternion rotation)
    {
        //Debug.Log("Camera Rotation Update Triggered");
        OnCameraRotationUpdate?.Invoke(rotation);

    }

}
