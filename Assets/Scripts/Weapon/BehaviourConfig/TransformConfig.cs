using UnityEngine;

[CreateAssetMenu(menuName = "WeaponBehaviourConfig/TransformConfig")]
public class TransformConfig : WeaponBehaviourConfig
{
    public override void Apply(GameObject gameObject, Weapon weapon)
    {
        var TransformComponent = gameObject.AddComponent<TransformComponent>();
        TransformComponent.Initialize(this, weapon);

    }
}
