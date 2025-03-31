using UnityEngine;

[CreateAssetMenu(menuName = "WeaponBehaviourConfig/RecoilConfig")]
public class RecoilConfig : WeaponBehaviourConfig
{
    public float RecoilAmount;
    public float KickbackAmount;
    public float RecoilRecoverySpeed;
    public override void Apply(GameObject gameObject, Weapon weapon)
    {
        var recoilComponent = gameObject.AddComponent<RecoilComponent>();
        recoilComponent.Initialize(this, weapon);

    }
}
