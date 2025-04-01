using Unity.VisualScripting;
using UnityEngine;

public class MeleeConfig : WeaponBehaviourConfig
{
    public override void Apply(GameObject gameObject, Weapon weapon)
    {
        var meleeComponent = weapon.AddComponent<MeleeComponent>();

    }
}
