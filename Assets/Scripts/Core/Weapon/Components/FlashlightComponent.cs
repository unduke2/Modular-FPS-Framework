using System;
using UnityEngine;

public class FlashlightComponent : BaseComponent<FlashlightConfig>
{
    private bool _isFlashlightOn;
    private GameObject _flashlightObject;


    public override void Initialize(FlashlightConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);
       
    }


    public void HandleFlashlight()
    {
        if (_isFlashlightOn)
        {
             _config.FlashlightObject.SetActive(false);
            _isFlashlightOn = false;
        }
        else
        {
            _config.FlashlightObject.SetActive(true);
            _isFlashlightOn = true;
        }
    }
}
