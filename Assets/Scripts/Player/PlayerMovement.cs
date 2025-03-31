using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStance _currentStance;

    private CharacterController _characterController;


    [Header("Movement Speeds")]
    [SerializeField] private float _walkSpeed = 2.5f;
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _crouchSpeed = 1f;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Rotation")]
    [SerializeField] private float _yawSensitivity = 1f;

    private Vector3 _velocity; // Tracks vertical movement (gravity + jumping)
    public Vector3 Velocity => _velocity;
    private Vector2 _directionInput; // Raw direction input
    private bool _jumpInput = false; // Tracks jump input state
    private float _currentSpeed;
    private float _currentYaw; // Tracks yaw rotation

    private Vector3 _currentMovementVelocity;
    public Vector3 CurrentMovementVelocity => _currentMovementVelocity;

    //private PlayerStance _previousStance; // Tracks the last stance

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _currentSpeed = _walkSpeed;
    }

    private void Update()
    {
        // Apply movement
        ProcessMovement();

        // Apply jump and gravity
        ProcessJumpAndGravity();

        //// Update stance
        //UpdateStance();
    }

    private void ProcessMovement()
    {
        // Calculate movement direction
        Vector3 forwardMovement = transform.forward * _directionInput.y;
        Vector3 strafeMovement = transform.right * _directionInput.x;
        Vector3 compositeMovement = (forwardMovement + strafeMovement).normalized;
        _currentMovementVelocity = compositeMovement * _currentSpeed;
        // Apply movement
        _characterController.Move(_currentMovementVelocity * Time.deltaTime);
    }

    private void ProcessJumpAndGravity()
    {
        // Check if grounded
        if (_characterController.isGrounded)
        {
            // Reset vertical velocity if grounded
            if (_velocity.y < 0f)
            {
                _velocity.y = 0f;
            }

            // Handle jumping
            if (_jumpInput)
            {
                _velocity.y = Mathf.Sqrt(-2f * _gravity * _jumpHeight); // Jump physics formula
            }
        }

        // Apply gravity
        _velocity.y += _gravity * Time.deltaTime;

        // Move the character controller vertically
        _characterController.Move(_velocity * Time.deltaTime);
    }

    public void HandleInput(MovementInput movementInput)
    {
        // Movement input
        _directionInput = movementInput.DirectionInput;

        // Handle jump input
        _jumpInput = movementInput.JumpInput;

        PlayerStance stance = PlayerStance.Stationary;

        // Update speed based on crouch/sprint
        if (movementInput.DirectionInput != Vector2.zero)
        {
            
            stance = movementInput.SprintInput ? PlayerStance.Sprinting : (movementInput.CrouchInput ? PlayerStance.Crouching : PlayerStance.Walking);

            if (_currentStance != stance)
            {
                Debug.Log("Updating player stance to: " + stance.ToString());

                _currentStance = stance;
                EventManager.TriggerPlayerStance(stance);
                UpdateSpeed();
            }
        }

        // Handle yaw rotation
        _currentYaw += movementInput.LookInputX * _yawSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, _currentYaw, 0f);
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
        }
    }


    public PlayerStance GetCurrentStance()
    {
        return _currentStance;
    }

    //private void UpdateStance()
    //{
    //    float speed = _characterController.velocity.magnitude;

    //    Debug.Log(speed);
    //}

    //private void UpdateStance()
    //{
    //    PlayerStance newStance;

    //    if (_currentSpeed == _runSpeed)
    //    {
    //        newStance = PlayerStance.Sprinting;
    //    }
    //    else if (_currentSpeed == _crouchSpeed)
    //    {
    //        newStance = PlayerStance.Crouching;
    //    }
    //    else if (_directionInput.magnitude > 0f)
    //    {
    //        newStance = PlayerStance.Walking;
    //    }
    //    else
    //    {
    //        newStance = PlayerStance.Stationary;
    //    }

    //    // If the stance has changed, trigger the event
    //    if (newStance != _previousStance)
    //    {
    //        _previousStance = newStance;
    //        CurrentStance = newStance;
    //        OnStanceChanged?.Invoke(newStance); // Broadcast stance change
    //    }
}
