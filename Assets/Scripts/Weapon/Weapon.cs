using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Weapon : MonoBehaviour
{

    private Dictionary<Type, MonoBehaviour> componentMap = new Dictionary<Type, MonoBehaviour>();
    private CameraController _holderCamera;
    public WeaponState State = new WeaponState();
    private Transform _weaponTargetTransform;
    public AudioSource AudioSource;

    public void Initialize(CameraController holderCamera, Transform weaponTargetTransform, AudioSource audioSource)
    {

        foreach (var component in GetComponentsInChildren<MonoBehaviour>()) // Search entire hierarchy
        {
            Debug.Log($"Weapon.Initialize: Found component {component.GetType()} on {component.gameObject.name}.");

            // Check if the component inherits from BaseComponent<TConfig> where TConfig : WeaponBehaviourConfig
            var baseType = component.GetType().BaseType; // Get the base type
            if (baseType != null && baseType.IsGenericType &&
                baseType.GetGenericTypeDefinition() == typeof(BaseComponent<>) &&
                typeof(WeaponBehaviourConfig).IsAssignableFrom(baseType.GetGenericArguments()[0]))
            {
                componentMap[component.GetType()] = component;
                Debug.Log($"Weapon.Initialize: Added {component.GetType()} to componentMap.");
            }
            else
            {
                Debug.LogWarning($"Weapon.Initialize: Skipping {component.GetType()} (not a valid BaseComponent<WeaponBehaviourConfig>).");
            }
        }
        State.AudioSource = audioSource;
        State.HolderCamera = holderCamera;
        State.IsReloading = false;
        SetWeaponTargetTransform( weaponTargetTransform );
        


        Debug.Log($"Weapon.Initialize: Total components mapped: {componentMap.Count}");
    }
    public T GetWeaponComponent<T>() where T : MonoBehaviour
    {
        foreach (var kvp in componentMap)
        {
            if (typeof(T).IsAssignableFrom(kvp.Key)) // Match derived types
            {
                return kvp.Value as T;
            }
        }
        return null;
    }



    public void HandleInput(WeaponInput input)
    {


        var fireComponent = GetWeaponComponent<FireComponent>();
        if (fireComponent != null)
        {
            if (input.Fire)
            {
                fireComponent.HandleFire();

            }
            else
            {
                fireComponent.HandleFireReleased();
            }
        }
        else
        {
            Debug.Log("FireComponent is null");
        }

        var reloadComponent = GetWeaponComponent<ReloadComponent>();
        if (reloadComponent != null)
        {

            if (input.Reload)
            {
                reloadComponent.HandleReload();
            }
        }

        var flashlightComponent = GetWeaponComponent<FlashlightComponent>();
        if (flashlightComponent != null)
        {
            if (input.ToggleFlashlight)
            {
                flashlightComponent.HandleFlashlight();
            }
        }

        // Aim logic - use the AimComponent to toggle ADS or HipFire mode
        var aimComponent = GetWeaponComponent<AimComponent>();
        if (aimComponent != null)
        {
            // Assume that when "Aim" is true, we're in ADS mode,
            // otherwise we're in HipFire mode.
            if (input.Aim)
            {
                aimComponent.SetAimingMode(AimingMode.ADS);
            }
            else
            {
                aimComponent.SetAimingMode(AimingMode.HipFire);
            }
        }

    }

    private void SetWeaponTargetTransform(Transform transform)
    {
        var transformComponent = GetWeaponComponent<TransformComponent>();
        if (transformComponent != null)
        {
            transformComponent.SetWeaponTargetTransform(transform);
        }
    }
}
