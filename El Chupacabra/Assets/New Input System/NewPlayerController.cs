using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewPlayerController : MonoBehaviour
{
    [SerializeField] GameObject _gameObject;
    [SerializeField] GameObject _Parent;
    [SerializeField] PauseUI _pause;
    [SerializeField] UIManager _uiManager;

    // Private Variables 
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _oldMoveDirection;
    private Vector3 _dashStartPosition;

    private float _moveSpeed = 10f;
    private float _jumpHeight = 5.0f;

    private float _edgeMovementSpeed = 5f;
    private float _edgeDetectionDistance = 20f;
    private float _edgeHangOffset = 0.5f;
    private float _dashSpeed = 10f;
    private float _dashDistance = 5f;

    [SerializeField] int _maxHp = 3;
    [SerializeField] int _currentHp;
    [SerializeField] int _score = 0;

    [SerializeField] bool _isWalking;
    [SerializeField] bool _isJumping = false;
    [SerializeField] bool _isDoubleJumping = false;
    [SerializeField] bool _isDashing = false;
    [SerializeField] bool _isGrounded = true;
    [SerializeField] bool _isFalling = false;
    [SerializeField] bool _isSpinAttack = false;

    [SerializeField] bool _isHangingMB = false;
    [SerializeField] bool _isHangingEdge = false;
    [SerializeField] bool _leavingMB = false;

    [Header("Movement Speeds")]
    private float _walkSpeed = 3.0f;
    private float _sprintMultiplier = 2.0f;

    [Header("Jump Parameters")]
    private float _jumpForce = 5.0f;
    private float _gravity = 9.81f;

    [Header("Look Sensitivity")]
    private float _mouseSensitivitiy = 2.0f;
    private float _upDownRange = 80.0f;

    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] CinemachineFreeLook _mainCamera;
    [SerializeField] PlayerInputHandler _inputHandler;

    private Vector3 _currentMovement;
    private float verticalRotation;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    private void Start()
    {
        _inputHandler = PlayerInputHandler.Instance;
        _currentHp = _maxHp;
        UpdateHP();
        UpdateScore();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        if (!_isGrounded && CheckIfShouldMove() && !_inputHandler.DoubleJumpTriggered)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
            _isFalling = true;
        }
        else if (_isGrounded && !_isJumping && !_isDoubleJumping && _inputHandler.JumpTriggered)
        {
            Jump();
            Debug.Log("First Jump");
        }
        else if (_isFalling && !_isDoubleJumping && _inputHandler.DoubleJumpTriggered)
        {
            DoubleJump();
            Debug.Log("Second jump");
        }
        else if (!_isDashing && _inputHandler.DashTriggered)
        {
            _isDashing = true;
            _dashStartPosition = transform.position;

        }
        else if (_inputHandler.SpinTriggered)
        {
            _isSpinAttack = true;
            Invoke("CancelSpin", 2);
        }
        else if (_isHangingEdge)
        {
            _isWalking = false;
            HangOnEdge();

        }
        else if (_isHangingMB && _inputHandler.JumpTriggered)
        {
            Debug.Log("let me out");
            _leavingMB = true;
            LeavingMonkeyBar();
        }
        else
        {
            if (CheckIfShouldMove() && _isGrounded)
            {
                _moveDirection = new Vector3(_inputHandler.MoveInput.x, 0.0f, _inputHandler.MoveInput.y);
                if (_moveDirection == Vector3.zero) { _isWalking = false; }
                else { _isWalking = true; }
                _moveDirection = transform.TransformDirection(_moveDirection);
                _moveDirection *= _moveSpeed;
                _oldMoveDirection = _moveDirection;
                _isJumping = false;
                _isDoubleJumping = false;
            }
        }
        if (_isDashing)
        {
            Dash();
        }
        if (CheckIfShouldMove())
        { _characterController.Move(_moveDirection * Time.deltaTime); }
        if (_inputHandler.PauseTriggered)
        {
            _pause.PauseGame();
        }

    }

    private void HandleRotation() // need to make a third person with cinemacine
    {
        /*   float mouseXRotation = _inputHandler.LookInput.x * _mouseSensitivitiy;
           verticalRotation -= _inputHandler.LookInput.y * mouseXRotation;
           verticalRotation = Mathf.Clamp(verticalRotation, -_upDownRange, _upDownRange);*/
        _mainCamera.m_XAxis.Value += _inputHandler.LookInput.x;
        _mainCamera.m_YAxis.Value += (_inputHandler.LookInput.y / 100);
    }
    private bool CheckIfShouldMove() // checks if the player is hanging on edge or haning on monkey bar and stops him from entering diffrent ifs
    {
        if (_isHangingEdge || _isHangingMB || _isDashing)
        {
            return false;
        }
        else return true;
    }

    private void Jump()
    {
        _isWalking = false;
        _moveDirection.y = _jumpHeight;
        _isJumping = true;
        _isGrounded = false;
    }
    private void DoubleJump()
    {
        _isWalking = false;
        _moveDirection.y = _jumpHeight;
        _isDoubleJumping = true;
        _isJumping = false;
        _isGrounded = false;
    }

    private void HangingEdge(Transform hit)
    {
        _isGrounded = false;

        _gameObject.transform.SetParent(hit.transform);
        _gameObject.transform.localPosition = Vector3.zero;

    }

    private void HangOnEdge()
    {

        Vector3 lateralMovement = new Vector3(_inputHandler.MoveInput.x, 0, 0).normalized * _edgeMovementSpeed;
        _characterController.Move(lateralMovement * Time.deltaTime);
        if (_inputHandler.JumpTriggered)
        {
            Debug.Log("let Go from Edge");
            _moveDirection = transform.forward * _jumpHeight;
            _moveDirection.y = _jumpHeight;
            _moveDirection.z = transform.forward.z;
            _isHangingEdge = false;
            _gameObject.transform.SetParent(_Parent.transform);
            _gameObject.transform.localPosition = Vector3.zero;
        }
        // Apply jump force away from the edge

    }
    private void OnMonkeyBar(Transform hit)
    {
        _isGrounded = false;

        _gameObject.transform.SetParent(hit.transform);
        _gameObject.transform.localPosition = Vector3.zero;
    }
    private void LeavingMonkeyBar()
    {
        _isGrounded = false;

        _gameObject.transform.SetParent(_Parent.transform);
        _gameObject.transform.localPosition = Vector3.zero;

        Vector3 lateralMovement = new Vector3(0, 0, 1f).normalized * _edgeMovementSpeed;
        _characterController.Move(lateralMovement * Time.deltaTime);
        _isHangingMB = false;
        _leavingMB = false;
    }
    private void Dash()
    {
        _isWalking = false;
        float distanceTraveled = Vector3.Distance(_dashStartPosition, transform.position);

        if (distanceTraveled < _dashDistance)
        {
            // Continue dashing
            _characterController.Move(transform.forward * _dashSpeed * Time.deltaTime);
        }
        else
        {
            // Stop dashing when the desired distance is reached
            _isDashing = false;
        }
    }
    private void CancelSpin()
    {
      _isSpinAttack = false;
    }
    private void StopMovement()
    {
        _moveDirection = Vector3.zero;

    }
    private void UpdateHP()
    {
        _uiManager.UpdateHP(_maxHp, _currentHp);
    }
    public void UpdateScore()
    {
        _uiManager.UpdateScore(_score);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.CompareTag("edge"))
        {
            _isHangingEdge = true;
            HangingEdge(hit.transform);
        }
        if (hit.gameObject.CompareTag("monkeyBar"))
        {
            Debug.Log("Hanging on monkey Bar");
            OnMonkeyBar(hit.transform);
            _isHangingMB = true;

        }
        if (hit.gameObject.CompareTag("ground"))
        {
            _isGrounded = true;
            _isFalling = false;
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.GamePlay;
        if (newGameState == GameState.GamePlay) { _inputHandler.gameObject.SetActive(true); }
            //_animator.speed = 1;
        else
        {
            _inputHandler.gameObject.SetActive(false);
            //_animator.speed = 0;
            StopMovement();
        }
    }

    public bool IsWalking
    {
        get { return _isWalking; }
        set { _isWalking = value; }
    }
    public bool IsJumping
    {
        get { return _isJumping; }
        set { _isJumping = value; }
    }
    public bool IsDoubleJumping
    {
        get { return _isDoubleJumping; }
        set { _isDoubleJumping = value; }
    }

    public bool IsGrounded
    {
        get { return _isGrounded; }
        set { _isGrounded = value; }
    }
    public bool IsHangingMB
    {
        get { return _isHangingMB; }
        set { _isHangingMB = value; }
    }
    public bool LeavingMB
    {
        get { return _leavingMB; }
        set { _leavingMB = value; }
    }
    public bool IsHangingEdge
    {
        get { return _isHangingEdge; }
        set { _isHangingEdge = value; }
    }
    public bool IsSpinAttacking
    {
        get { return _isSpinAttack; }
        set { _isSpinAttack = value; }
    }
    public bool IsDashing
    {
        get { return _isDashing; }
        set { _isDashing = value; }
    }



}
