using UnityEngine;

public class SpreadComponent : BaseComponent<SpreadConfig>
{
    private float _currentSpread;

    private float _targetSpread;

    private float _stanceMultiplier = 1f;

    private float _adsMultiplier = 1f;

    private float _spreadRecoveryMultiplier = 1f;

    public override void Initialize(SpreadConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);

        _currentSpread = _config.BaseSpread;
        UpdateTargetSpread();
    }


    private void Update()
    {
        if (Mathf.Abs(_currentSpread - _targetSpread) > 0.01f)
        {
            _currentSpread = Mathf.Lerp(
                _currentSpread,
                _targetSpread,
                _config.SpreadUpdateSpeed * _spreadRecoveryMultiplier * Time.deltaTime
                );
        }
        else
        {
            _currentSpread = _targetSpread;
        }
    }


    private void SetStanceSpread(PlayerStance stance)
    {
        if (_config.SpreadMultipliers.TryGetValue(stance, out float multiplier))
        {
            _stanceMultiplier = multiplier;
            Debug.Log($"{nameof(SpreadComponent)}: Updating stance multiplier to {multiplier}");
        }
        else
        {
            Debug.LogWarning($"{nameof(SpreadComponent)}: No spread multiplier found for stance {stance}");
            _stanceMultiplier = 1f;
        }

        UpdateTargetSpread();
    }


    public void SetAimModeSpread(float spreadMultiplier, AimingMode aimingMode)
    {
        switch (aimingMode)
        {
            case AimingMode.ADS:
                _adsMultiplier = spreadMultiplier;
                Debug.Log($"{nameof(SpreadComponent)}: Setting ADS multiplier to {spreadMultiplier}");
                break;
                //Add more as needed
        }

        UpdateTargetSpread();
    }


    private void UpdateTargetSpread()
    {
        _targetSpread = _config.BaseSpread * _stanceMultiplier * _adsMultiplier;
        Debug.Log($"{nameof(SpreadComponent)}: New target spread: {_targetSpread}");
    }


    public Vector3 CalculateSpread(Vector3 direction, Vector3 right, Vector3 up)
    {
        float spreadX = Random.Range(-_currentSpread, _currentSpread);
        float spreadY = Random.Range(-_currentSpread, _currentSpread);

        Vector3 offset = right * spreadX + up * spreadY;

        return (direction + offset).normalized;
    }
}
