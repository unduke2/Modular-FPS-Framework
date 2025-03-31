using System.Collections;
using System.Linq;
using UnityEngine;

public class FireComponent : BaseComponent<FireConfig>
{

    private float _nextFireTime = 0f;
    private FireMode _currentFireMode;
    private bool _canFire = true;
    private Coroutine _burstFireCoroutine = null;
    //private LayerMask _layerMask = LayerMask.GetMask("Level", "Enemy");

    private Transform weaponTransform;



    public override void Initialize(FireConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);
        if (_config.FireModes == null || _config.FireModes.Length == 0)
        {
            Debug.LogError("FireConfig: FireModes array is empty. Weapon cannot fire!");
            _currentFireMode = FireMode.None; // Default to a safe "None" mode if no valid fire modes are provided.
            return;
        }
        //_currentFireMode = _config.FireModes.FirstOrDefault();
        _currentFireMode = _config.DefaultFireMode;
        Debug.Log($"FireComponent: Initialized with FireMode {_currentFireMode} and FireRate {_config.FireRate}.");

    }
    public void HandleFire()
    {
        if (_canFire)
        {


            _canFire = false;

            if (_currentFireMode == FireMode.None)
            {
                Debug.LogWarning("FireComponent: Attempted to fire with no valid fire mode.");
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

        if (InCooldown())
        {
            return;
        }

        FireBullet();
        _nextFireTime = Time.time + (1f / _config.FireRate);


    }

    public void FireBurst()
    {
        if (InCooldown())
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
        if (InCooldown())
        {
            return;
        }

        FireBullet();
        _nextFireTime = Time.time + (1f / _config.FireRate);
    }


    private IEnumerator BurstFireRoutine()
    {
        for (int i = _config.BurstCount; i > 0; i--)
        {
            if (_weapon.State.CurrentAmmo < 0)
            {

                break;
            }

            FireBullet();

            yield return new WaitForSeconds(1f / _config.FireRate);

        }
        _burstFireCoroutine = null;
        _nextFireTime = Time.time + (1f / _config.FireRate);
    }

    private void Update()
    {
        //Debug.DrawRay(_weapon.State.MuzzleTransform.position, _weapon.State.MuzzleTransform.forward);
    }
    private void FireBullet()
    {
        if (_weapon.State.IsReloading)
        {
            return ;
        }
        if (_weapon.State.CurrentAmmo > 0)
        {


            Vector3 bulletDir = _weapon.State.MuzzleTransform.forward;
            var spreadComponent = _weapon.GetWeaponComponent<SpreadComponent>();

            if (spreadComponent != null)
            {

                bulletDir = spreadComponent.CalculateSpread(bulletDir, transform.right, transform.up);

            }

            var recoilComponent = _weapon.GetWeaponComponent<RecoilComponent>();

            if (recoilComponent != null)
            {
                //Debug.Log("applied recoil");
                recoilComponent.ApplyRecoil();
            }

            Debug.Log("Final bullet direction: " + bulletDir);
            RaycastHit hit;

            if (Physics.Raycast(_weapon.State.MuzzleTransform.position, bulletDir, out hit, _config.HitRange))
            {
                string layer = LayerMask.LayerToName(hit.collider.gameObject.layer);
                Debug.Log("Hit " + layer);
                if (_config.BulletHolePrefab != null)
                {
                    // Create a rotation that aligns the decal with the surface normal.
                    Quaternion decalRotation = Quaternion.LookRotation(-hit.normal);
                    // Slightly offset the decal along the normal to prevent z-fighting.
                    Vector3 decalPosition = hit.point + hit.normal * _config.BulletHoleOffset;
                    Debug.Log("Instantiating bullet hole at: " + decalPosition);

                    GameObject bulletHole = Instantiate(_config.BulletHolePrefab, decalPosition, decalRotation);

                    //// Optionally, parent the decal to the hit object so it moves if the object moves.
                    //bulletHole.transform.parent = hit.collider.transform;

                    // Optionally destroy the bullet hole after some time to avoid clutter.
                    Destroy(bulletHole, 10f);
                }
            }
            else
            {
                Debug.Log("Did not hit");
            }


            ////GameObject bullet = Instantiate(bullet, weaponTransform.position, Quaternion.LookRotation(bulletDirection, weaponTransform.up));
            GameObject flash = Instantiate(
              _config.MuzzleFlashPrefab,
              _weapon.State.MuzzleTransform.position,
              _weapon.State.MuzzleTransform.rotation);
            flash.transform.parent = _weapon.State.MuzzleTransform;
            Destroy(flash, 0.5f);

            var audioSource = _weapon.State.AudioSource;
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }


          _weapon.State.CurrentAmmo--;
            Debug.Log("Fired a bullet! Current ammo count: " + _weapon.State.CurrentAmmo);
        } 

    }



    private bool InCooldown()
    {
        return Time.time < _nextFireTime ? true : false;
    }


}
