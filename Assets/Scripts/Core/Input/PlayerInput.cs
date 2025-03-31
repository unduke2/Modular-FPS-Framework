using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    public Vector2 DirectionInput { get; private set; }

    public bool JumpInput { get; private set; }

    public bool CrouchInput { get; private set; }

    public bool SprintInput { get; private set; }

    public Vector2 MousePosInput { get; private set; }

    public bool FireInput { get; private set; }

    public bool ReloadInput { get; private set; }

    public bool FreeLookInput { get; private set; }

    public bool AimInput { get; private set; }

    public bool FlashLightInput { get; set; }

    [SerializeField] private InputActionReference _directionActionRef;
    [SerializeField] private InputActionReference _jumpActionRef;
    [SerializeField] private InputActionReference _sprintActionRef;
    [SerializeField] private InputActionReference _crouchActionRef;
    [SerializeField] private InputActionReference _mousePosActionRef;
    [SerializeField] private InputActionReference _fireActionRef;
    [SerializeField] private InputActionReference _reloadActionRef;
    [SerializeField] private InputActionReference _freeLookActionRef;
    [SerializeField] private InputActionReference _aimActionRef;
    [SerializeField] private InputActionReference _flashLightActionRef;

    public InputAction _directionAction => _directionActionRef;
    public InputAction _jumpAction => _jumpActionRef;

    public InputAction _crouchAction => _crouchActionRef;

    public InputAction _sprintAction => _sprintActionRef;

    public InputAction _mousePosAction => _mousePosActionRef;
    public  InputAction _fireAction => _fireActionRef;
    public InputAction _reloadAction => _reloadActionRef;
    public InputAction _freeLookAction => _freeLookActionRef;
    public InputAction _aimAction => _aimActionRef;
    public InputAction _flashLightAction => _flashLightActionRef;

    private void Awake()
    {
        _directionAction.performed += OnMove;
        _directionAction.canceled += OnMove;

        _jumpAction.performed += OnJump;
        _jumpAction.canceled += OnJump;

        _sprintAction.performed += OnSprint;
        _sprintAction.canceled += OnSprint;

        _crouchAction.performed += OnCrouch;
        _crouchAction.canceled += OnCrouch;

        _fireAction.performed += ctx => FireInput = true;
        _fireAction.canceled += ctx => FireInput = false;

        _reloadAction.performed += OnReload;
        _reloadAction.canceled += OnReload;

        _freeLookAction.performed += OnFreeLook;
        _freeLookAction.canceled += OnFreeLook;

        _aimAction.performed += OnAim;
        _aimAction.canceled += OnAim;

        _flashLightAction.performed += OnFlashlight;
        _flashLightAction.canceled += OnFlashlight;

        
    }


    private void Update()
    {
        //Debug.Log($"Movement Input Vector: {MovementInput}, Jump Input State: {JumpInput}, Crouch Input State: {CrouchInput}, Sprint Input State: {SprintInput}");

        MousePosInput = _mousePosAction.ReadValue<Vector2>();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            DirectionInput = _directionAction.ReadValue<Vector2>();
           
        } else
        {
            DirectionInput = Vector2.zero;
        }
    }

    //private void OnFire(InputAction.CallbackContext ctx)
    //{
        
    //    FireInput = ctx.performed;
    //}

    private void OnReload(InputAction.CallbackContext ctx)
    { 
        ReloadInput = ctx.performed;
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        JumpInput = ctx.performed;
    }

    private void OnSprint(InputAction.CallbackContext ctx)
    {
        SprintInput = ctx.performed;
    }

    private void OnCrouch(InputAction.CallbackContext ctx)
    {
        CrouchInput = ctx.performed;
    }

    private void OnFreeLook(InputAction.CallbackContext ctx)
    {
        FreeLookInput = ctx.performed;
    }

    private void OnAim(InputAction.CallbackContext ctx)
    {
        AimInput = ctx.performed;
    }

    private void OnFlashlight(InputAction.CallbackContext ctx)
    {
      FlashLightInput = ctx.performed;
    }


    private void OnEnable()
    {
        _directionAction.Enable();
        _jumpAction.Enable();
        _crouchAction.Enable();
        _sprintAction.Enable();
        _mousePosAction.Enable();
        _fireAction.Enable();
        _reloadAction.Enable();  
        _freeLookAction.Enable();
        _aimAction.Enable();
        _flashLightAction.Enable();
    }
    private void OnDisable()
    {
        _directionAction.Disable();
        _jumpAction.Disable();
        _crouchAction.Disable();
        _sprintAction.Disable();
        _mousePosAction.Disable();
        _fireAction.Disable();
        _reloadAction.Disable();
        _freeLookAction.Disable();
        _aimAction.Disable();
        _flashLightAction.Disable();    
    }

}
