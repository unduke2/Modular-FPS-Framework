using UnityEngine;
using UnityEngine.Animations.Rigging;


public class Player : MonoBehaviour
{

    private PlayerMovement _playerMovement;

    private IKConstraints _constraints;
    private RigBuilder _rigBuilder;

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

        _constraints = GetComponentInChildren<IKConstraints>();
        _rigBuilder = GetComponentInChildren<RigBuilder>();



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

        if (_currentWeapon != null && _constraints != null)
        {
            _constraints.SetIKConstraintTargets(_currentWeapon.transform.Find("LeftIKWeapon_mag"), _currentWeapon.transform.Find("LeftIKWeapon_slide"), _currentWeapon.transform.Find("LeftIKWeapon_handle"));
            _rigBuilder.Build();
        }
  

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
            Debug.LogWarning($"{nameof(Player)}: No PlayerInput found.");
            return;
        }
        HandleMovementInput();
        HandleWeaponInput();

        if (_playerMovement != null)
        {
            _animator.SetFloat("Velocity", _playerMovement.HorizontalVelocity.magnitude);
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
            Debug.LogWarning($"{nameof(Player)}: No weapon available to handle input.");
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
            Debug.LogWarning($"{nameof(Player)}: No PlayerMovement script found.");
            return;
        }

        _playerMovement.HandleInput(new MovementInput
        {
            DirectionInput = _input.DirectionInput,
            SprintInput = _input.SprintInput,
            JumpInput = _input.JumpInput,
            LookInputX = _input.MousePosInput.x,
            CrouchInput = _input.CrouchInput
        });
    }
}