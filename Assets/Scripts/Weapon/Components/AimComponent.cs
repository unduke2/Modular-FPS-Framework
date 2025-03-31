using UnityEngine;

public class AimComponent : BaseComponent<AimConfig>
{
    private AimingModeData _currentAimingModeData;
    private AimingMode _currentAimingMode;

    public override void Initialize(AimConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);
    }



    public void SetAimingMode(AimingMode mode)
    {
        // Only update if the mode changes.
        if (_currentAimingMode == mode)
            return;
        
        _currentAimingMode = mode;
        // Retrieve the data for the current aiming mode.
        var data = _config.AimingData.GetAimingModeData(_currentAimingMode);
        if (data != null)
        {
            _currentAimingModeData = data;

            WeaponEvents.ChangeAimingModeSpread(data.SpreadMultiplier);

            // Update camera FOV, sensitivity, etc.
            var cameraController = _weapon.State.HolderCamera;
            if (cameraController != null)
            {
                cameraController.SetFOV(data.FieldOfView, data.TransitionSpeed);
                // Optionally adjust camera sensitivity, etc.
            }
            else
            {
                Debug.LogWarning("Camera controller is null");
            }
            
            // Notify animation component (which can use PositionOffset/RotationOffset if needed)
            var animationComponent = _weapon.GetWeaponComponent<AnimationComponent>();
            if (animationComponent != null)
            {
                // For ADS, we might use an additive ADS animation or IK adjustment.
                bool isADS = (_currentAimingMode == AimingMode.ADS);
                animationComponent.SetADS(isADS);
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

