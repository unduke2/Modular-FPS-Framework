using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string WeaponName;
    public GameObject WeaponPrefab;
    public List<WeaponBehaviourConfig> BehaviourConfigs; 
}
