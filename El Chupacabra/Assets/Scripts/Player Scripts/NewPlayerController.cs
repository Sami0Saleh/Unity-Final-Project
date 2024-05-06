using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewPlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    //[SerializeField] Transform _mainCameraTransform;
    [SerializeField] PlayerInputHandler _inputHandler;
    [SerializeField] GameObject _gameObject;
    [SerializeField] GameObject _Parent;
    [SerializeField] PauseUI _pause;
    [SerializeField] UIManager _uiManager;
    [SerializeField] GameEndManager _gameEndManager;
    [SerializeField] Animator _animator;

    // Private Variables 
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _oldMoveDirection;
    private Vector3 _dashStartPosition;

    private float _moveSpeed = 10f;
    private float _jumpHeight = 5.0f;
    private float _dashSpeed = 10f;
    private float _dashDistance = 5f;
    private float RotateSpeed = 3;


    [SerializeField] int _maxHp = 3;
    [SerializeField] int _currentHp;
    [SerializeField] int _maxScore = 0;
    [SerializeField] int _score = 0;
    [SerializeField] int _maxEnemyCount = 0;
    [SerializeField] int _enemyCount = 0;

    [SerializeField] bool _isWalking;
    [SerializeField] bool _isJumping = false;
    [SerializeField] bool _isDoubleJumping = false;
    [SerializeField] bool _isDashing = false;
    [SerializeField] bool _isGrounded = true;
    [SerializeField] bool _isFalling = false;
    [SerializeField] bool _isSpinAttack = false;

    [SerializeField] bool _leavingMB;
    [SerializeField] bool _isHangingEdge;
    [SerializeField] bool _isHangingMB;
    [SerializeField] bool _isPaused = false;
    [SerializeField] float _gravity = 9.81f;

    private void Awake()
    { 
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        GameStateManager.Instance.SetState(GameState.GamePlay);

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
        UpdateEnemyCount();
    }
    private void Update()
    {
        HandleMovement();
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
        }
        else if (_isFalling && !_isDoubleJumping && _inputHandler.DoubleJumpTriggered)
        {
            DoubleJump();
        }
        else if (!_isDashing && _inputHandler.DashTriggered)
        {
            _isDashing = true;
            _dashStartPosition = transform.position;

        }
        else if (_inputHandler.SpinTriggered)
        {
            _isSpinAttack = true;
            StartCoroutine(StopSpinAttack());
        }
        else if (_isHangingEdge)
        {
            _isWalking = false;
            HangOnEdge();

        }
        else if (_isHangingMB && _inputHandler.JumpTriggered)
        {
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
                HandleRotation();
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
        if (!_isPaused && _inputHandler.PauseTriggered)
        {
            _pause.PauseGame();
        }
        else if (_isPaused && _inputHandler.PauseTriggered)
        {
            _pause.ContinueGame();
        }
        if (_enemyCount == _maxEnemyCount && _score == _maxScore)
        {
            _gameEndManager.GameWon();
        }
    }
    private void HandleRotation() // need to make a third person with cinemacine
    {
        if (_inputHandler.LookInput.magnitude > 0)
        {
            float angle = Mathf.Atan2(_inputHandler.LookInput.y, _inputHandler.LookInput.x) * Mathf.Rad2Deg;

            // Rotate the player towards the calculated angle
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);

        }
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

    public void TakeDamage()
    {
        _currentHp --;
        UpdateHP();
        if (_currentHp == 0)
        {
            Die();
        }
    }
    private void Die()
    {
        _gameEndManager.PlayerDead();
    }
    public void UpdateHP()
    {
        _uiManager.UpdateHP(_maxHp, _currentHp);
    }
    public void UpdateEnemyCount()
    {
        _uiManager.UpdateEnemy(_maxEnemyCount, _enemyCount);
    }
    public void UpdateScore()
    {
        _uiManager.UpdateScore(_maxScore , _score);
    }

    IEnumerator StopSpinAttack()
    {
        yield return new WaitForSecondsRealtime(1);
        _isSpinAttack = false;
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.GamePlay;
        if (newGameState == GameState.GamePlay)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _animator.speed = 1;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;

            _animator.speed = 0;
        }
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
            OnMonkeyBar(hit.transform);
            _isHangingMB = true;
        }
        if (hit.gameObject.CompareTag("damage") && !_isDashing && !_isSpinAttack)
        {
            TakeDamage();
        }
        if (hit.gameObject.CompareTag("ground"))
        {
            _isGrounded = true;
            _isFalling = false;
        }
        if (hit.gameObject.CompareTag("enemy") && _isSpinAttack)
        {
            BaseEnemy enemy = hit.gameObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
        if (hit.gameObject.CompareTag("enemy") && _isDashing)
        {
            BaseEnemy enemy = hit.gameObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
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
    public int MaxHp
    {
        get { return _maxHp; }
        set { _maxHp = value; }
    }
    public int CurrentHp
    {
        get { return _currentHp; }
        set { _currentHp = value; }
    }
    public int MaxScore
    {
        get { return _maxScore; }
        set { _maxScore = value; }
    }
    public int Score
    {
        get { return _score; }
        set { _score = value; }
    }
    public int MaxEnemyCount
    {
        get { return _maxEnemyCount; }
        set { _maxEnemyCount = value; }
    }
    public int EnemyCount
    {
        get { return _enemyCount; }
        set { _enemyCount = value; }
    }
    public bool IsPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }

    public bool IsFalling
    {
        get { return _isFalling; }
        set { _isFalling = value; }
    }

    private void HangingEdge(Transform hit)
    {
        _isGrounded = false;

        _gameObject.transform.SetParent(hit.transform);
        _gameObject.transform.localPosition = Vector3.zero;

    }
    private void HangOnEdge()
    {

        Vector3 lateralMovement = new Vector3(_inputHandler.MoveInput.x, 0, 0).normalized;
        _characterController.Move(lateralMovement * Time.deltaTime);
        if (_inputHandler.JumpTriggered)
        {
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

        Vector3 lateralMovement = new Vector3(0, 0, 1f).normalized;
        _characterController.Move(lateralMovement * Time.deltaTime);
        _isHangingMB = false;
        _leavingMB = false;
    }
}
