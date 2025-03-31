using UnityEngine;

public class AnimationComponent : BaseComponent<AnimationConfig>
{
    private Animator _animator;
    private bool _canPlayReload = true;
    private bool _isReloading;
    private ReloadComponent _reloadComponent;

    public override void Initialize(AnimationConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);

        _animator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        _reloadComponent = _weapon.GetWeaponComponent<ReloadComponent>();
    }

    public void PlayReloadAnimation()
    {
        if (_canPlayReload && !_isReloading)
        {
            _isReloading = true;
            _canPlayReload = false;

            _weapon.State.IsReloading = true;

            SetADS(false);

            _animator.SetTrigger("Reload");

            Debug.Log("Playing reload animation");
        }
    }
    public void OnReloadAnimationComplete()
    {

        if (!_isReloading)
        {
            return;
        }

        Debug.Log("Completed reload animation");

        if (_reloadComponent != null)
        {
            _reloadComponent.CompleteReload();
        }

        _isReloading = false;
        _canPlayReload = true;
        _weapon.State.IsReloading = false;
    }

    public void SetADS(bool ads)
    {
        if (_animator != null)
        {
            _animator.SetBool("ADS", ads);
        }
    }

}