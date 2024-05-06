using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] public InputActionAsset _playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string _actionMapName = "PlayerActionMap";

    [Header("Action Name References")]
    [SerializeField] private string _move = "Move";
    [SerializeField] private string _look = "Look";
    [SerializeField] private string _jump = "Jump";
    [SerializeField] private string _doubleJump = "DoubleJump";
    [SerializeField] private string _dash = "Dash";
    [SerializeField] private string _spin = "Spin";
    [SerializeField] private string _sprint = "Sprint";
    [SerializeField] private string _pause = "Pause";

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _doubleJumpAction;
    private InputAction _dashAction;
    private InputAction _spinAction;
    private InputAction _sprintAction;
    private InputAction _pauseAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool DoubleJumpTriggered { get; private set; }
    public bool DashTriggered { get; private set; }
    public bool SpinTriggered { get; private set; }
    public bool PauseTriggered { get; private set; }

    public float SprintValue { get; private set; }


    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _moveAction = _playerControls.FindActionMap(_actionMapName).FindAction(_move);
        _lookAction = _playerControls.FindActionMap(_actionMapName).FindAction(_look);
        _jumpAction = _playerControls.FindActionMap(_actionMapName).FindAction(_jump);
        _doubleJumpAction = _playerControls.FindActionMap(_actionMapName).FindAction(_doubleJump);
        _dashAction = _playerControls.FindActionMap(_actionMapName).FindAction(_dash);
        _spinAction = _playerControls.FindActionMap(_actionMapName).FindAction(_spin);
        _sprintAction = _playerControls.FindActionMap(_actionMapName).FindAction(_sprint);
        _pauseAction = _playerControls.FindActionMap(_actionMapName).FindAction(_pause);

        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        _moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        _moveAction.canceled += context => MoveInput = Vector2.zero;

        _lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        _lookAction.canceled += context => LookInput = Vector2.zero;

        _jumpAction.started += context => JumpTriggered = true; 
        _jumpAction.canceled += context => JumpTriggered = false;

        _doubleJumpAction.started += context => DoubleJumpTriggered = true;
        _doubleJumpAction.canceled += context => DoubleJumpTriggered = false;

        _dashAction.started += context => DashTriggered = true;
        _dashAction.canceled += context => DashTriggered = false;

        _spinAction.started += context => SpinTriggered = true;
        _spinAction.canceled += context => SpinTriggered = false;

        _pauseAction.started += context => PauseTriggered = true;
        _pauseAction.canceled += context => PauseTriggered = false;

        _sprintAction.started += context => SprintValue = context.ReadValue<float>();
        _sprintAction.canceled += context => SprintValue = 0f;
    }
    private void OnEnable()
    {
        _moveAction.Enable();
        _lookAction.Enable();
        _jumpAction.Enable();
        _doubleJumpAction.Enable();
        _dashAction.Enable();
        _spinAction.Enable();
        _sprintAction.Enable();
        _pauseAction.Enable();

    }

    private void OnDisable()
    {
        _moveAction?.Disable();
        _lookAction?.Disable();
        _jumpAction?.Disable();
        _doubleJumpAction?.Disable();
        _dashAction?.Disable();
        _spinAction?.Disable();
        _sprintAction?.Disable();
        _pauseAction?.Disable();
    }

     

}
