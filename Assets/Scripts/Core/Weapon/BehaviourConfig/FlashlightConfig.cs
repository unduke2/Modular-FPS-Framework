using UnityEngine;

[CreateAssetMenu(menuName = "WeaponBehaviourConfig/FlashlightConfig")]
public class FlashlightConfig : WeaponBehaviourConfig
{
    public AudioClip _flashlightEnabled;
    public AudioClip _flashlightDisabled;

    public override void Apply(GameObject weaponObject, Weapon weapon)
    {
        var flashlightComponent = weaponObject.AddComponent<FlashlightComponent>();
        flashlightComponent.Initialize(this, weapon);
    }
}
