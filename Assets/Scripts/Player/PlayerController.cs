using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;


public class PlayerController : MonoBehaviour
{

    private PlayerMovement _playerMovement;
    private Anchor _anchor;
    private PlayerInput _input;
    private Weapon _currentWeapon;
    private WeaponInput _weaponInput;
    private MovementInput _movementInput;
    private IKManager _ikManager;
    [SerializeField] private CameraController _cameraController;
    private IKRig _ikRig;
    private RigBuilder _rigBuilder;



    [Header("Weapon Settings")]
    [SerializeField] private WeaponData _weaponData;
    private Animator _animator;
    [SerializeField] private Transform _weaponTargetTransform;
    private AudioSource _audioSource;

    

    //[Header("Camera Settings")]
    //[SerializeField] private CinemachineCamera _cinemachineCamera;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();

        _animator = GetComponentInChildren<Animator>();

        _input = GetComponent<PlayerInput>();
        _weaponInput = new WeaponInput();
        _movementInput = new MovementInput();

        _anchor = GetComponentInChildren<Anchor>();

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
        if (_input != null)
        {
            HandleMovementInput();
            HandleWeaponInput();
        } else
        {
            Debug.Log("Input is null");
        }

        _animator.SetFloat("Velocity", _playerMovement.CurrentMovementVelocity.magnitude);


    }

    private void LateUpdate()
    {
        _anchor.UpdatePitch(_input.MousePosInput.y);
    }

    private void HandleWeaponInput()
    {
        _weaponInput.Fire = _input.FireInput;
        _weaponInput.Reload = _input.ReloadInput;
        _weaponInput.ToggleFlashlight = _input.FlashLightInput;
        _weaponInput.Aim = _input.AimInput;

        if (_currentWeapon != null)
        {
            _currentWeapon.HandleInput(_weaponInput);
        }
         else
        {
            Debug.Log("Weapon is null");
        }
    }
    private void HandleMovementInput()
    {
        _movementInput.DirectionInput = _input.DirectionInput;
        _movementInput.SprintInput = _input.SprintInput;
        _movementInput.JumpInput = _input.JumpInput;
        _movementInput.LookInputX = _input.MousePosInput.x;
        _movementInput.CrouchInput = _input.CrouchInput;
        if (_playerMovement != null)
        {
            _playerMovement.HandleInput(_movementInput);
        }
    }

  
}