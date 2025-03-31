using UnityEngine;

public abstract class WeaponBehaviourConfig : ScriptableObject
{
    public abstract void Apply(GameObject gameObject, Weapon weapon);
}
