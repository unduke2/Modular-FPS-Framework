using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponBehaviourConfig/SpreadConfig")]
public class SpreadConfig : WeaponBehaviourConfig
{
    public float BaseSpread;
    //public float SpreadIncreasePerShot;
    public float SpreadRecoveryRate;
    public float SpreadUpdateSpeed;
    public float MaxSpread;

    public float walkingSpreadMulti;
    public float sprintingSpreadMulti;
    public float stationarySpreadMulti;
    public float crouchingSpreadMulti;

    public Dictionary<PlayerStance, float> SpreadMultipliers => new Dictionary<PlayerStance, float>
    {
        { PlayerStance.Stationary, stationarySpreadMulti },
        { PlayerStance.Crouching,  crouchingSpreadMulti  },
        { PlayerStance.Walking,    walkingSpreadMulti    },
        { PlayerStance.Sprinting,  sprintingSpreadMulti  },

    };

    public override void Apply(GameObject weaponObject, Weapon weapon)
    {
        var spreadComponent = weaponObject.AddComponent<SpreadComponent>();
        spreadComponent.Initialize(this, weapon);


    }
}
