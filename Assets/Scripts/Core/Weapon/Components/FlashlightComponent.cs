using UnityEngine;

public class FlashlightComponent : BaseComponent<FlashlightConfig>
{
    private bool _isFlashlightOn = false;
    private GameObject _flashlight;
    private bool _canToggle;

    public override void Initialize(FlashlightConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);
        _flashlight = _weapon.transform.Find("Flashlight").GetChild(0).gameObject;
    }


    public void HandleFlashlightPressed()
    {
        if (_flashlight == null)
        {
            Debug.LogWarning($"{nameof(FlashlightComponent)}: Flashlight object is missing");
            return;
        }

        if (_canToggle)
        {
            _canToggle = false;

            _isFlashlightOn = !_isFlashlightOn;

            AudioClip audioClip = _isFlashlightOn ? _config._flashlightEnabled : _config._flashlightDisabled;
            _weapon.State.FlashlightAudioSource.PlayOneShot(audioClip);

            _flashlight.SetActive(_isFlashlightOn);
        }
    }

    public void HandleFlashlightReleased()
    {
        _canToggle = true;
    }
}
