using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action<PlayerStance> OnStanceChanged;

    [Header("Movement Speeds")]
    [SerializeField] private float _walkSpeed = 2.5f;
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _crouchSpeed = 1f;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Rotation")]
    [SerializeField] private float _yawSensitivity = 1f;

    private CharacterController _characterController;

    private PlayerStance _currentStance;

    private Vector3 _verticalVelocity;
    private Vector3 _horizontalVelocity;
    private Vector2 _directionInput;

    private bool _jumpInput = false;

    private float _currentSpeed;
    private float _currentYaw;

    public Vector3 HorizontalVelocity => _horizontalVelocity;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (_characterController == null)
        {
            Debug.LogError($"{nameof(PlayerMovement)}: No CharacterController found on this GameObject");
        }
        _currentSpeed = _walkSpeed;
        _currentStance = PlayerStance.Stationary;
    }


    private void Update()
    {
        ProcessMovement();
        ProcessJumpAndGravity();
    }


    public void HandleInput(MovementInput movementInput)
    {
        _directionInput = movementInput.DirectionInput;

        _jumpInput = movementInput.JumpInput;

        PlayerStance newStance = PlayerStance.Stationary;

        if (movementInput.DirectionInput != Vector2.zero)
        {

            newStance = movementInput.SprintInput ? PlayerStance.Sprinting : (movementInput.CrouchInput ? PlayerStance.Crouching : PlayerStance.Walking);

            if (_currentStance != newStance)
            {
                _currentStance = newStance;
                OnStanceChanged?.Invoke(_currentStance);
                UpdateSpeed();
                Debug.Log($"{nameof(PlayerMovement)}: Stance changed to {_currentStance}");
            }
        }

        _currentYaw += movementInput.LookInputX * _yawSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, _currentYaw, 0f);
    }


    private void ProcessMovement()
    {

        if (_characterController == null) return;

        Vector3 forwardMovement = transform.forward * _directionInput.y;
        Vector3 strafeMovement = transform.right * _directionInput.x;
        Vector3 compositeMovement = (forwardMovement + strafeMovement).normalized;

        _horizontalVelocity = compositeMovement * _currentSpeed;

        _characterController.Move(_horizontalVelocity * Time.deltaTime);
    }


    private void ProcessJumpAndGravity()
    {
        if (_characterController == null) return;

        if (_characterController.isGrounded)
        {
            if (_verticalVelocity.y < 0f)
            {
                _verticalVelocity.y = 0f;
            }

            if (_jumpInput)
            {
                _verticalVelocity.y = Mathf.Sqrt(-2f * _gravity * _jumpHeight);
            }
        }

        _verticalVelocity.y += _gravity * Time.deltaTime;

        _characterController.Move(_verticalVelocity * Time.deltaTime);
    }


    private void UpdateSpeed()
    {

        switch (_currentStance)
        {
            case PlayerStance.Walking:
                _currentSpeed = _walkSpeed;
                break;
            case PlayerStance.Crouching:
                _currentSpeed = _crouchSpeed;
                break;
            case PlayerStance.Sprinting:
                _currentSpeed = _runSpeed;
                break;
            case PlayerStance.Stationary:
                _currentSpeed = 0f;
                break;
        }
    }

    public PlayerStance GetCurrentStance()
    {
        return _currentStance;
    }

}
