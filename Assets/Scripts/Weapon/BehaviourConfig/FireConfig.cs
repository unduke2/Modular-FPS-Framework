using UnityEngine;

[CreateAssetMenu(menuName = "WeaponBehaviourConfig/FireConfig")]
public class FireConfig : WeaponBehaviourConfig
{
    public FireMode[] FireModes;
    public float FireRate;
    public FireMode DefaultFireMode;
    public int BurstCount;
    public float HitRange = 200f;
    public GameObject MuzzleFlashPrefab;
    public GameObject BulletHolePrefab;
    public float BulletHoleOffset;

    public override void Apply(GameObject weaponObject, Weapon weapon)
    {
        Debug.Log($"FireConfig: Applying FireComponent to {weaponObject.name}.");

        var firingComponent = weaponObject.AddComponent<FireComponent>();
        if (firingComponent != null)
        {
            Debug.Log("FireConfig: Successfully added FireComponent.");
            firingComponent.Initialize(this, weapon);
        }
        else
        {
            Debug.LogError("FireConfig: Failed to add FireComponent.");
        }
    }
}
