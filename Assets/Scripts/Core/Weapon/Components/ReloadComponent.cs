using System;
using UnityEngine;

public class ReloadComponent : BaseComponent<ReloadConfig>
{
    private int _totalAmmo;

    private AudioSource _magazineAudioSource;
    private AnimationComponent _animationComponent;
    public override void Initialize(ReloadConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);

        _totalAmmo = _config.TotalAmmo;
        _weapon.State.CurrentAmmo = _config.MagazineSize;
        _weapon.State.IsReloading = false;
        
    }

    private void Start()
    {
        _animationComponent = _weapon.GetWeaponComponent<AnimationComponent>();
        _magazineAudioSource = _weapon.State.MagazineAudioSource;
    }
    public void HandleReload()
    {
      if (_weapon.State.IsReloading || 
        _weapon.State.CurrentAmmo == _config.MagazineSize || 
        _totalAmmo <= 0)
    {
        return;
    }

        if (_magazineAudioSource != null && _config.ReloadSound != null)
        {
            _magazineAudioSource.PlayOneShot(_config.ReloadSound);
        }

        if (_animationComponent != null)
        {
            _animationComponent.PlayReloadAnimation();
        }
        else
        {
            Debug.LogError($"{nameof(ReloadComponent)}: No AnimationComponent found on this weapon");
        }
    }

    public void CompleteReload()
    {
        if (!_weapon.State.IsReloading)
        {
            return;
        }

        int amountToReload = _config.MagazineSize - _weapon.State.CurrentAmmo;
        if (_totalAmmo >= amountToReload)
        {
            _totalAmmo -= amountToReload;
            _weapon.State.CurrentAmmo = _config.MagazineSize;
        }
        else
        {
            _weapon.State.CurrentAmmo += _totalAmmo;
            _totalAmmo = 0;
        }
        Debug.Log($"Weapon reloaded! Current Ammo: {_weapon.State.CurrentAmmo} Total Ammo: {_totalAmmo}");
    }

}
