using UnityEngine;

public class AimComponent : BaseComponent<AimConfig>
{
    private AimingModeData _currentAimingModeData;
    private AimingMode _currentAimingMode;
    private AnimationComponent _animationComponent;

    public override void Initialize(AimConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);
    }

    private void Start()
    {
        _animationComponent = _weapon.GetWeaponComponent<AnimationComponent>();
    }


    public void SetAimingMode(AimingMode mode)
    {
        if (_currentAimingMode == mode)
            return;
        
        _currentAimingMode = mode;


        var data = _config.AimingData.GetAimingModeData(_currentAimingMode);
        if (data != null)
        {
            _currentAimingModeData = data;

            WeaponEvents.ChangeAimingModeSpread(data.SpreadMultiplier);

            var cameraController = _weapon.State.HolderCamera;
            if (cameraController != null)
            {
                cameraController.SetFOV(data.FieldOfView, data.TransitionSpeed);
            }
            else
            {
                Debug.LogWarning("Camera controller is null");
            }
            
            if (_animationComponent != null)
            {
                bool isADS = (_currentAimingMode == AimingMode.ADS);
                _animationComponent.SetADS(isADS);
            }
        }
        else
        {
            Debug.LogWarning("Aiming mode data not found for mode: " + mode);
        }
    }

    public AimingModeData GetCurrentAimingModeData()
    {
        return _currentAimingModeData;
    }
}

