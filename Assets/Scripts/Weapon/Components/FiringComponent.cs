using System.Collections;
using UnityEngine;

public class FireComponent : BaseComponent<FireConfig>
{

    private float _nextFireTime;
    private bool _canFire = true;
    private Coroutine _burstFireCoroutine;


    private FireMode _currentFireMode;
    private SpreadComponent _spreadComponent;
    private RecoilComponent _recoilComponent;

    public override void Initialize(FireConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);

        if (_config.FireModes == null || _config.FireModes.Length == 0)
        {
            Debug.LogError("FireConfig: FireModes array is empty. Weapon cannot fire!");
            _currentFireMode = FireMode.None;
            return;
        }

        _currentFireMode = _config.DefaultFireMode;
        Debug.Log($"FireComponent: Initialized with FireMode {_currentFireMode} and FireRate {_config.FireRate}.");
    }

    private void Start()
    {
        _spreadComponent = _weapon.GetWeaponComponent<SpreadComponent>();
        _recoilComponent = _weapon.GetWeaponComponent<RecoilComponent>();
    }

    public void HandleFire()
    {
        if (_canFire)
        {
            _canFire = false;

            if (_currentFireMode == FireMode.None)
            {
                Debug.LogWarning($"{nameof(FireComponent)}: Attempted to fire with no valid fire mode.");
                return;
            }

            switch (_currentFireMode)
            {
                case FireMode.Single:
                    FireSingle();
                    break;
                case FireMode.Burst:
                    FireBurst();
                    break;
            }
        }

        if (_currentFireMode == FireMode.Auto)
        {
            FireAuto();
        }
    }


    public void HandleFireReleased()
    {
        _canFire = true;
    }


    public void FireSingle()
    {

        if (!CanShootAgain())
        {
            return;
        }

        FireBullet();
        _nextFireTime = Time.time + (1f / _config.FireRate);


    }


    public void FireBurst()
    {
        if (!CanShootAgain())
        {
            return;
        }

        if (_burstFireCoroutine != null)
        {
            StopCoroutine(_burstFireCoroutine);
            _burstFireCoroutine = null;
        }
        _burstFireCoroutine = StartCoroutine(BurstFireRoutine());
    }


    public void FireAuto()
    {
        if (!CanShootAgain())
        {
            return;
        }

        FireBullet();
        _nextFireTime = Time.time + (1f / _config.FireRate);
    }


    private IEnumerator BurstFireRoutine()
    {
        int shotsRemaining = _config.BurstCount;

        while (shotsRemaining > 0)
        {
            if (_weapon.State.CurrentAmmo <= 0) break;

            FireBullet();
            shotsRemaining--;

            yield return new WaitForSeconds(1f / _config.FireRate);
        }

        _nextFireTime = Time.time + (1f / _config.FireRate);
        _burstFireCoroutine = null;
    }


    private void FireBullet()
    {
        if (_weapon.State.IsReloading)
        {
            return;
        }

        if (_weapon.State.CurrentAmmo <= 0)
        {
            Debug.Log("No ammo left!");
        }



        Vector3 muzzleForward = _weapon.State.MuzzleTransform.forward;
        Vector3 bulletDir = muzzleForward;

        if (_spreadComponent != null)
        {
            bulletDir = _spreadComponent.CalculateSpread(bulletDir, transform.right, transform.up);
        }

        if (_recoilComponent != null)
        {
            _recoilComponent.ApplyRecoil();
        }

        if (Physics.Raycast(_weapon.State.MuzzleTransform.position, bulletDir, out RaycastHit hit, _config.HitRange))
        {
            Debug.Log($"Hit {LayerMask.LayerToName(hit.collider.gameObject.layer)}");

            if (_config.BulletHolePrefab != null)
            {
                Quaternion decalRotation = Quaternion.LookRotation(-hit.normal);

                Vector3 decalPosition = hit.point + hit.normal * _config.BulletHoleOffset;

                GameObject bulletHole = Instantiate(_config.BulletHolePrefab, decalPosition, decalRotation);

                Destroy(bulletHole, 10f);
            }
        }
        else
        {
            Debug.Log("No hit");
        }

        GameObject flash = Instantiate(
          _config.MuzzleFlashPrefab,
          _weapon.State.MuzzleTransform.position,
          _weapon.State.MuzzleTransform.rotation);

        flash.transform.parent = _weapon.State.MuzzleTransform;

        Destroy(flash, 0.5f);

        AudioSource gunshotAudioSource = _config.GunshotAudioSource;
        if (gunshotAudioSource != null && gunshotAudioSource.clip != null)
        {
            gunshotAudioSource.PlayOneShot(gunshotAudioSource.clip);
        }


        _weapon.State.CurrentAmmo--;
        Debug.Log("Fired a bullet! Current ammo: " + _weapon.State.CurrentAmmo);
    }


    private bool CanShootAgain()
    {
        return Time.time >= _nextFireTime;
    }
}
