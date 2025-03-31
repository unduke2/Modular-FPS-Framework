using System;
using UnityEngine;

public class ReloadComponent : BaseComponent<ReloadConfig>
{
    private int _totalAmmo;
    public override void Initialize(ReloadConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);
        _totalAmmo = _config.TotalAmmo;
        _weapon.State.CurrentAmmo = _config.MagazineSize;
        _weapon.State.IsReloading = false;
        
    }
    public void HandleReload()
    {
      if (_weapon.State.IsReloading || 
        _weapon.State.CurrentAmmo == _config.MagazineSize || 
        _totalAmmo <= 0)
    {
        return;
    }


        WeaponEvents.Reload();
        Debug.Log("Reloading Started!");
        _weapon.State.IsReloading = true;
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
        Debug.Log($"Weapon has been reloaded! Current Ammo: {_weapon.State.CurrentAmmo} Total Ammo: {_totalAmmo}");
    }

}
