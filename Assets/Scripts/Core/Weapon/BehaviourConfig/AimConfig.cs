using UnityEngine;

[CreateAssetMenu(menuName = "WeaponBehaviourConfig/AimConfig")]
public class AimConfig : WeaponBehaviourConfig
{
    public AimingData AimingData;
    public override void Apply(GameObject weaponObject, Weapon weapon)
    {
        var aimComponent = weaponObject.AddComponent<AimComponent>();
        aimComponent.Initialize(this, weapon);


    }
}
