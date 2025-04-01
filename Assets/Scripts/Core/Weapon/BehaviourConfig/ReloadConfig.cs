using UnityEngine;

[CreateAssetMenu(menuName = "WeaponBehaviourConfig/ReloadConfig")]
public class ReloadConfig : WeaponBehaviourConfig
{
    public int MagazineSize;
    public int TotalAmmo = 50;

    public AudioClip ReloadSound;
    public override void Apply(GameObject weaponObject, Weapon weapon)
    {
        var reloadComponent = weaponObject.AddComponent<ReloadComponent>();
        reloadComponent.Initialize(this, weapon);


    }
}
