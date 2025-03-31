using UnityEngine;

public abstract class BaseComponent<TConfig> : MonoBehaviour where TConfig : WeaponBehaviourConfig
{
    protected Weapon _weapon;
    protected TConfig _config;

    public virtual void Initialize(TConfig config, Weapon weapon)
    {
        _weapon = weapon;
        _config = config;
    }
}
