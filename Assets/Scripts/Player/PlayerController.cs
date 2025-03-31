using UnityEngine;


public class PlayerController : MonoBehaviour
{

    private PlayerMovement _playerMovement;

    private CameraTarget _cameraTarget;

    private Animator _animator;

    private PlayerInput _input;
    private WeaponInput _weaponInput;
    private MovementInput _movementInput;

    private Weapon _currentWeapon;

    [SerializeField] private CameraController _cameraController;

    [Header("Weapon Settings")]
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _weaponTargetTransform;


    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();

        _animator = GetComponentInChildren<Animator>();

        _input = GetComponent<PlayerInput>();
        _weaponInput = new WeaponInput();
        _movementInput = new MovementInput();

        _cameraTarget = GetComponentInChildren<CameraTarget>();

        _currentWeapon = WeaponFactory.CreateWeapon(_weaponData,
            _animator.GetBoneTransform(HumanBodyBones.RightHand).Find("WeaponSocket"),
            _cameraController,
            _weaponTargetTransform)
            .GetComponent<Weapon>();

        if (_currentWeapon == null)
        {
            Debug.LogError("Weapon creation failed! Check WeaponData or WeaponFactory.");
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        if (_input == null)
        {
            Debug.LogWarning($"{nameof(PlayerController)}: No PlayerInput found.");
            return;
        }
        HandleMovementInput();
        HandleWeaponInput();

        if (_playerMovement != null)
        {
            _animator.SetFloat("Velocity", _playerMovement.CurrentMovementVelocity.magnitude);
        }
    }


    private void LateUpdate()
    {
        if (_cameraTarget != null && _input != null)
        {
            _cameraTarget.UpdatePitch(_input.MousePosInput.y);
        }
    }


    private void HandleWeaponInput()
    {
        if (_currentWeapon == null)
        {
            Debug.LogWarning($"{nameof(PlayerController)}: No weapon available to handle input.");
            return;
        }

        _currentWeapon.HandleInput(new WeaponInput
        {
            Fire = _input.FireInput,
            Reload = _input.ReloadInput,
            ToggleFlashlight = _input.FlashLightInput,
            Aim = _input.AimInput,
        });
    }


    private void HandleMovementInput()
    {
        if (_playerMovement == null)
        {
            Debug.LogWarning($"{nameof(PlayerController)}: No PlayerMovement script found.");
            return;
        }

        _playerMovement.HandleInput(new MovementInput
        {
            DirectionInput = _movementInput.DirectionInput,
            SprintInput = _movementInput.SprintInput,
            JumpInput = _movementInput.JumpInput,
            LookInputX = _movementInput.LookInputX,
            CrouchInput = _movementInput.CrouchInput
        });
    }
}