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

    // Private Variables 
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _oldMoveDirection;
    private float _gravity = 20.0f;

    private float _horizontalInput;
    private float _verticalInput;
    private float _moveSpeed = 10f;
    private float _jumpHeight = 10f;

    private float _edgeMovementSpeed = 5f;
    private float _edgeDetectionDistance = 20f;
    private float _edgeHangOffset = 0.5f;

    private bool _isWalking;
    private bool _isJumping = false;
    public bool _isDoubleJumping = false;
    private bool _isDashing = false;
    public bool _isGrounded = true;
    private bool _isSpinAttack = false;

    public bool _isHangingMB = false;
    private bool _isHangingEdge = false;
    public bool _leavingMB = false;


    
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _isGrounded = Physics.CheckSphere(transform.position, 0.01f, _groundLayer);

        if (!_isGrounded && !Input.GetButtonDown("Jump") && CheckIfShouldMove())
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
            Debug.Log("Falling");
        }
        else if (_isHangingEdge)
        {
            _isWalking = false;
            HangingEdge();
        }
        else if (_isHangingMB && Input.GetButtonDown("Jump"))
        {
            Debug.Log("let me out");
            _leavingMB = true;
        }
        else
        {
            if (CheckIfShouldMove())
            { PlayerMovement(); }
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
    }

    private void HangingEdge()
    {
        Vector3 currentPos = transform.position;
        Debug.Log("Detected a edge");
        RaycastHit hit;
        // Perform raycast to detect edges below the character
        if (Physics.Raycast(transform.position, -transform.up, out hit, _edgeDetectionDistance, _edgeLayer))
        {

            Physics.Raycast(transform.position, -transform.up, out hit, _edgeDetectionDistance, _edgeLayer);
            Debug.Log("Entered RayCast");
            // Position character at the edge with slight offset
            Vector3 hangPosition = hit.point + transform.up * _edgeHangOffset;
            _characterController.Move(/*hangPosition*/ currentPos - transform.position);

            // Disable movement along y-axis
            _moveDirection.y = 0;

            // Check for lateral movement input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 lateralMovement = new Vector3(horizontalInput, 0, verticalInput).normalized * _edgeMovementSpeed;
            _characterController.Move(lateralMovement * Time.deltaTime);
        }
        else
        {
            // Stop hanging if no edge is detected
            _isHangingEdge = false;
        }

        // Check for jump input to vault from edge
        if (Input.GetButtonDown("Jump"))
        {

            Debug.Log("let Go from Edge");
            JumpFromEdge();
        }
    }

    private void JumpFromEdge()
    {
        // Apply jump force away from the edge
        _moveDirection = transform.forward * _jumpHeight;
        _moveDirection.y = _jumpHeight;
        _moveDirection.z = transform.forward.z;
        _isHangingEdge = false;
    }
    private void OnMonkeyBar(Transform hit)
    {
        _isGrounded = false;
        
        _gameObject.transform.SetParent(hit.transform);
        _gameObject.transform.localPosition = Vector3.zero;

        if (_leavingMB)
        { return; }

        
        
    }
    private bool CheckIfShouldMove() // checks if the player is hanging on edge or haning on monkey bar and stops him from entering diffrent ifs
    {
        if (_isHangingEdge || _isHangingMB)
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
        }
        else if (hit.gameObject.CompareTag("monkeyBar"))
        {
            Debug.Log("Hanging on monkey Bar");
            OnMonkeyBar(hit.transform);
            _isHangingMB = true;

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
}
