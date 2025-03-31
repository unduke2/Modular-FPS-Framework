using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationComponent : BaseComponent<AnimationConfig>
{
    private Animator _animator;
    private bool _canPlayReload = true;
    private bool _isReloading;

    public override void Initialize(AnimationConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);
        _animator = GetComponentInParent<Animator>();

    }
    private void OnEnable()
    {
        AnimationEventProxy.OnReloadComplete += OnReloadAnimationComplete;
        WeaponEvents.OnReload += PlayReloadAnimation;

    }

    private void OnDisable()
    {
        AnimationEventProxy.OnReloadComplete -= OnReloadAnimationComplete;
        WeaponEvents.OnReload -= PlayReloadAnimation;
    }

    public void PlayReloadAnimation()
    {
        if (_canPlayReload)
        {
            SetADS(false);
            _isReloading = true;
            _canPlayReload = false;
    

            _animator.SetTrigger("Reload");
            Debug.Log("Playing reload animation");
        }
    }

    public void SetADS(bool ads)
    {
        _animator.SetBool("ADS", ads);
    }
    public void OnReloadAnimationComplete()
    {
        Debug.Log("Completed reload animation");

        // Update the ammo by calling CompleteReload from the ReloadComponent
        var reloadComponent = _weapon.GetWeaponComponent<ReloadComponent>();
        if (reloadComponent != null)
        {
            reloadComponent.CompleteReload();
        }

        _isReloading = false;
        _canPlayReload = true;
        _weapon.State.IsReloading = false;
    }



}