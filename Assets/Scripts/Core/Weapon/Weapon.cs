using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Weapon : MonoBehaviour
{

    private Dictionary<Type, MonoBehaviour> componentMap = new Dictionary<Type, MonoBehaviour>();

    public WeaponState State = new WeaponState();

    private FireComponent _fireComponent;
    private ReloadComponent _reloadComponent;
    private AimComponent _aimComponent;
    private FlashlightComponent _flashLightComponent;


    public void Initialize(CameraController holderCamera, Transform weaponTargetTransform)
    {

        foreach (var component in GetComponentsInChildren<MonoBehaviour>())
        {
            Debug.Log($"Weapon.Initialize: Found component {component.GetType()} on {component.gameObject.name}.");

            var baseType = component.GetType().BaseType;
            if (baseType != null && 
                baseType.IsGenericType &&
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

        State.HolderCamera = holderCamera;
        State.IsReloading = false;

        _fireComponent = GetWeaponComponent<FireComponent>();
        _reloadComponent = GetWeaponComponent<ReloadComponent>();
        _aimComponent = GetWeaponComponent<AimComponent>();
        _flashLightComponent = GetWeaponComponent<FlashlightComponent>();
        
        Debug.Log($"Weapon.Initialize: Total components mapped: {componentMap.Count}");
    }


    public T GetWeaponComponent<T>() where T : MonoBehaviour
    {
        foreach (var kvp in componentMap)
        {
            if (typeof(T).IsAssignableFrom(kvp.Key))
            {
                return kvp.Value as T;
            }
        }
        return null;
    }


    public void HandleInput(WeaponInput input)
    {

        if (_fireComponent != null)
        {
            if (input.Fire)
            {
                _fireComponent.HandleFire();

            }
            else
            {
                _fireComponent.HandleFireReleased();
            }
        }

        if (_reloadComponent != null)
        {

            if (input.Reload)
            {
                _reloadComponent.HandleReload();
            }
        }

        if (_flashLightComponent != null)
        {
            if (input.ToggleFlashlight)
            {
                _flashLightComponent.HandleFlashlightPressed();
            }
            else
            {
                _flashLightComponent.HandleFlashlightReleased();
            }
        }

        if (_aimComponent != null)
        {
            if (input.Aim)
            {
                _aimComponent.SetAimingMode(AimingMode.ADS);
            }
            else
            {
                _aimComponent.SetAimingMode(AimingMode.HipFire);
            }
        }
    }
}
