using UnityEngine;

public class SpreadComponent : BaseComponent<SpreadConfig>
{
    private float _currentSpread;
    private float _targetSpread;

    // Store separate multipliers.
    private float _stanceMultiplier = 1f;
    private float _adsMultiplier = 1f;

    // Multiplier to speed up recovery
    public float SpreadRecoveryMultiplier = 1f;

    public override void Initialize(SpreadConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);
        EventManager.OnPlayerStanceUpdate += SetTargetSpread;
        // Start with base values
        _currentSpread = _config.BaseSpread;
        _stanceMultiplier = 1f;
        _adsMultiplier = 1f;
        UpdateTargetSpread();
    }

    private void OnEnable()
    {
        WeaponEvents.OnAimingModeSpreadChanged += OnAimingModeSpreadChanged;
    }
    private void OnDisable()
    {
        WeaponEvents.OnAimingModeSpreadChanged -= OnAimingModeSpreadChanged;
    }

    private void SetTargetSpread(PlayerStance stance)
    {
        // Update stance multiplier based on stance
        if (_config.SpreadMultipliers.TryGetValue(stance, out float multiplier))
        {
            Debug.Log("Updated spread based on player stance");
            _stanceMultiplier = multiplier;
        }
        else
        {
            Debug.LogWarning($"Spread multiplier for stance {stance} not found. Using default (1).");
            _stanceMultiplier = 1f;
        }
        UpdateTargetSpread();
    }

    public void OnAimingModeSpreadChanged(float spreadMultiplier)
    {
        Debug.Log($"Spread change event received: {spreadMultiplier}");
        _targetSpread = _config.BaseSpread * spreadMultiplier;
    }

    // Combine the multipliers to update the target spread.
    private void UpdateTargetSpread()
    {
        _targetSpread = _config.BaseSpread * _stanceMultiplier * _adsMultiplier;
        Debug.Log($"New target spread: {_targetSpread}");
    }

    private void Update()
    {
        if (Mathf.Abs(_currentSpread - _targetSpread) > 0.01f)
        {
            _currentSpread = Mathf.Lerp(_currentSpread, _targetSpread, _config.SpreadUpdateSpeed * SpreadRecoveryMultiplier * Time.deltaTime);
        }
        else
        {
            _currentSpread = _targetSpread;
        }
    }

    public Vector3 CalculateSpread(Vector3 direction, Vector3 right, Vector3 up)
    {
        float spreadX = Random.Range(-_currentSpread, _currentSpread);
        float spreadY = Random.Range(-_currentSpread, _currentSpread);
        Vector3 offset = right * spreadX + up * spreadY;
        return (direction + offset).normalized;
    }
}
