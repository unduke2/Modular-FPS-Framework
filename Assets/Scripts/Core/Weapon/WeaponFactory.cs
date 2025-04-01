using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponFactory : MonoBehaviour
{
    public static GameObject CreateWeapon(WeaponData data, Transform holderTransform, CameraController holderCamera, Transform weaponTargetTransform)
    {
        GameObject weaponObject = Instantiate(data.WeaponPrefab, holderTransform);
        
        Weapon weapon = weaponObject.GetComponent<Weapon>() ?? weaponObject.AddComponent<Weapon>();

        var muzzle = weaponObject.transform.Find("Muzzle");
        weapon.State.MuzzleTransform = muzzle;
        weapon.State.GunshotAudioSource = muzzle.GetComponent<AudioSource>();

        foreach (var config in data.BehaviourConfigs)
        {
            if (config == null)
            {
                Debug.LogError("WeaponFactory: Null config found in BehaviorConfigs.");
                continue;
            }

            Debug.Log($"WeaponFactory: Applying config of type {config.GetType()}.");
            config.Apply(weaponObject, weapon);
        }
        weapon.Initialize(holderCamera, weaponTargetTransform);

        return weaponObject;
    }
}
