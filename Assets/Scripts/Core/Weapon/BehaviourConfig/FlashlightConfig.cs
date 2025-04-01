using UnityEngine;

[CreateAssetMenu(menuName = "WeaponBehaviourConfig/FlashlightConfig")]
public class FlashlightConfig : WeaponBehaviourConfig
{
    public GameObject FlashlightObject;

    public override void Apply(GameObject weaponObject, Weapon weapon)
    {
        var flashlightComponent = weaponObject.AddComponent<FlashlightComponent>();
        flashlightComponent.Initialize(this, weapon);
    }
}
