using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
 
    [Header("Layer Masks")]
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] LayerMask _edgeLayer;
    [SerializeField] LayerMask _monkeyBarLayer;

    [SerializeField] GameObject _gameObject;
    [SerializeField] GameObject _Parent;


    // Private Variables 
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _oldMoveDirection;
    private Vector3 _dashStartPosition;
    private float _gravity = 20.0f;

    private float _horizontalInput;
    private float _verticalInput;
    private float _moveSpeed = 10f;
    private float _jumpHeight = 10f;

    private float _edgeMovementSpeed = 5f;
    private float _edgeDetectionDistance = 20f;
    private float _edgeHangOffset = 0.5f;
    private float _dashSpeed = 10f;
    private float _dashDistance = 5f;

    [SerializeField] bool _isWalking;
    [SerializeField] bool _isJumping = false;
    [SerializeField] bool _isDoubleJumping = false;
    [SerializeField] bool _isDashing = false;
    [SerializeField] bool _isGrounded = true;
    [SerializeField] bool _isSpinAttack = false;

    [SerializeField] bool _isHangingMB = false;
    [SerializeField] bool _isHangingEdge = false;
    [SerializeField] bool _leavingMB = false;


    
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        /*_isGrounded = Physics.CheckSphere(transform.position, 0.01f, _groundLayer);*/

        if (!_isGrounded && !Input.GetButtonDown("Jump") && CheckIfShouldMove())
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
            Debug.Log("Falling");
        }
        else if (_isHangingEdge)
        {
            _isWalking = false;
            HangOnEdge();
            
        }
        else if (_isHangingMB && Input.GetButtonDown("Jump"))
        {
            Debug.Log("let me out");
            _leavingMB = true;
            LeavingMonkeyBar();
        }
        else if (!_isJumping && !_isDoubleJumping && Input.GetButtonDown("Jump"))
        {
            Jump();
            Debug.Log("wow i'm jumping");
        }
        else if (_isJumping && Input.GetButtonDown("Jump"))
        {
            DoubleJump();
            Debug.Log("wow i'm jumping again");
        }
        else if (!_isDashing && Input.GetKey(KeyCode.Q))
        {
            _isDashing = true;
            _dashStartPosition = transform.position;
            
        }
        else if (Input.GetKey(KeyCode.E))
        {
            _isSpinAttack = true;
        }
        else
        {
            if (CheckIfShouldMove())
            { PlayerMovement(); }
        }
        if (_isDashing)
        {
            Dash();
        }
        if (CheckIfShouldMove())
        { _characterController.Move(_moveDirection * Time.deltaTime); }
    }

    private void PlayerMovement()
    {
        _moveDirection = new Vector3(_horizontalInput, 0.0f, _verticalInput);
        if (_moveDirection == Vector3.zero) { _isWalking = false; }
        else { _isWalking = true; }
        _moveDirection = transform.TransformDirection(_moveDirection);
        _moveDirection *= _moveSpeed;
        _oldMoveDirection = _moveDirection;
        _isJumping = false;
        _isDoubleJumping = false;
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
        float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");
        Vector3 lateralMovement = new Vector3(horizontalInput, 0, 0).normalized * _edgeMovementSpeed;
        _characterController.Move(lateralMovement * Time.deltaTime);
        if (Input.GetButtonDown("Jump"))
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
    private void SpinAttack()
    {

        _isSpinAttack = false;
    }
    private bool CheckIfShouldMove() // checks if the player is hanging on edge or haning on monkey bar and stops him from entering diffrent ifs
    {
        if (_isHangingEdge || _isHangingMB || _isDashing)
        {
            return false;
        }
        else return true;
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
        }
    }


    // Properties
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
