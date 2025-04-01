using UnityEngine;

[CreateAssetMenu(menuName = "WeaponBehaviourConfig/AnimationConfig")]
public class AnimationConfig : WeaponBehaviourConfig
{
    public float RightHandIKWeight = 1.0f;
    public float LeftHandIKWeight = 1.0f;
    public HandIKOffset RightHandOffset;
    public HandIKOffset LeftHandOffset;
    public override void Apply(GameObject weaponObject, Weapon weapon)
    {
        var animationComponent = weaponObject.AddComponent<AnimationComponent>();
        animationComponent.Initialize(this, weapon);


    }
}
[System.Serializable]
public class HandIKOffset
{
    public Vector3 RotationOffset;
}