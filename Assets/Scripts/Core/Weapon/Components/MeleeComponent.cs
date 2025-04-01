using Unity.VisualScripting;
using UnityEngine;
using System;

public class MeleeComponent : BaseComponent<MeleeConfig>
{
    public override void Initialize(MeleeConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);



    }
    public void Execute()
    {
        Debug.Log("Performed melee attack");
    }
}
