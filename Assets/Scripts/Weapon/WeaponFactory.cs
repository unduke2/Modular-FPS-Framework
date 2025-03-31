using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Animations.Rigging;

public class WeaponFactory : MonoBehaviour
{
    public static GameObject CreateWeapon(WeaponData data, Transform holderTransform, CameraController holderCamera, Transform weaponTargetTransform, AudioSource audioSource)
    {
        GameObject weaponObject = Instantiate(data.WeaponPrefab, holderTransform);

        Weapon weapon = weaponObject.GetComponent<Weapon>() ?? weaponObject.AddComponent<Weapon>();

        weapon.State.MuzzleTransform = weaponObject.transform.Find("Muzzle");




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
        weapon.Initialize(holderCamera, weaponTargetTransform, audioSource);

        return weaponObject;
    }









}
