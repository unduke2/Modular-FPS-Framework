using UnityEngine;

public class RecoilComponent : BaseComponent<RecoilConfig>
{
    private float _currentRecoilOffset;

    private CameraController _cameraController;

    public override void Initialize(RecoilConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);
      
    }


    private void Start()
    {
        _cameraController = _weapon.State.HolderCamera;

        if (_cameraController == null)
        {
            Debug.LogWarning($"{nameof(RecoilComponent)}: No CameraController assigned in WeaponState");
        }
    }


    private void Update()
    {
        RecoverRecoil();
    }


    public void ApplyRecoil()
    {
        _currentRecoilOffset -= _config.RecoilAmount;

        if (_cameraController != null)
        {
            _cameraController.ApplyRecoil(_currentRecoilOffset / 2f);
        }
        else
        {
            Debug.Log("Camera controller is null - cannot apply recoil offset.");
        }
    }


    public void RecoverRecoil()
    {
        //Gotta try Mathf.MoveTowards for a different feel if needed
        _currentRecoilOffset = Mathf.Lerp(
            _currentRecoilOffset,
            0f, 
            Time.deltaTime * _config.RecoilRecoverySpeed
            );

        if (_cameraController != null)
        {
            _cameraController.RecoverRecoil(_config.RecoilRecoverySpeed);
        }
    }
}

