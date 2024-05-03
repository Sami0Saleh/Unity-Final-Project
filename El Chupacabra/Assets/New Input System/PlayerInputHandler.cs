using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset _playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string _actionMapName = "PlayerActionMap";

    [Header("Action Name References")]
    [SerializeField] private string _move = "Move";
    [SerializeField] private string _look = "Look";
    [SerializeField] private string _jump = "Jump";
    [SerializeField] private string _dash = "Dash";
    [SerializeField] private string _spin = "Spin";
    [SerializeField] private string _sprint = "Sprint";

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _spinAction;
    private InputAction _sprintAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool DashTriggered { get; private set; }
    public bool SpinTriggered { get; private set; }

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
        _dashAction = _playerControls.FindActionMap(_actionMapName).FindAction(_dash);
        _spinAction = _playerControls.FindActionMap(_actionMapName).FindAction(_spin);
        _sprintAction = _playerControls.FindActionMap(_actionMapName).FindAction(_sprint);

        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        _moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        _moveAction.canceled += context => MoveInput = Vector2.zero;

        _lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        _lookAction.canceled += context => LookInput = Vector2.zero;

        _jumpAction.performed += context => JumpTriggered = true;
        _jumpAction.canceled += context => JumpTriggered = false;

        _dashAction.performed += context => DashTriggered = true;
        _dashAction.canceled += context => DashTriggered = false;

        _spinAction.performed += context => SpinTriggered = true;
        _spinAction.canceled += context => SpinTriggered = false;

        _sprintAction.performed += context => SprintValue = context.ReadValue<float>();
        _sprintAction.canceled += context => SprintValue = 0f;
    }
    private void OnEnable()
    {
        _moveAction.Enable();
        _lookAction.Enable();
        _jumpAction.Enable();
        _dashAction.Enable();
        _spinAction.Enable();
        _sprintAction.Enable();

    }

    private void OnDisable()
    {
        _moveAction?.Disable();
        _lookAction?.Disable();
        _jumpAction?.Disable();
        _dashAction?.Disable();
        _spinAction?.Disable();
        _sprintAction?.Disable();

    }

     

}
