using UnityEngine;

public class RecoilComponent : BaseComponent<RecoilConfig>
{
    // This value represents the accumulated recoil (affecting pitch).
    private float _currentRecoilOffset;


    public override void Initialize(RecoilConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);
      
    }

    public void ApplyRecoil()
    {
        // Increase the recoil offset by the configured amount.
        _currentRecoilOffset -= _config.RecoilAmount;

        // Get the camera controller from the weapon.
        var cameraController = _weapon.State.HolderCamera;
        if (cameraController != null)
        {
            // Pass the recoil offset to the camera controller.
            // You might want to adjust the divisor based on desired strength.
            cameraController.ApplyRecoil(_currentRecoilOffset / 2f);
        }
        else
        {
            Debug.Log("Camera controller is null");
        }

        //var transformComponent = _weapon.GetWeaponComponent<TransformComponent>();
        //if (transformComponent != null)
        //{
        //    transformComponent.ApplyKickback(new Vector3(0f, 0f, - _config.KickbackAmount), 10f);

        //}

    }

    private void Update()
    {
        RecoverRecoil();
    }

    public void RecoverRecoil()
    {
        // Smoothly recover the recoil offset toward zero.
        _currentRecoilOffset = Mathf.Lerp(_currentRecoilOffset, 0f, Time.deltaTime * _config.RecoilRecoverySpeed);

        var cameraController = _weapon.State.HolderCamera;
        if (cameraController != null)
        {
            cameraController.RecoverRecoil(_config.RecoilRecoverySpeed);
        }

        //// NEW: Also recover kickback by setting its target offset to zero.
        //var transformComponent = _weapon.GetWeaponComponent<TransformComponent>();
        //if (transformComponent != null)
        //{
        //    var aimingComponent = _weapon.GetWeaponComponent<AimComponent>();
        //    if (aimingComponent != null)
        //    {
        //        transformComponent.ApplyKickback(aimingComponent.GetCurrentAimingModeData().PositionOffset, _config.RecoilRecoverySpeed);
        //    }
        //}
    }
}

