using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{

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
    [SerializeField] CinemachineVirtualCamera _mainCamera;
    [SerializeField] PlayerInputHandler _inputHandler;

    private Vector3 _currentMovement;
    private float verticalRotation;

    private void Start()
    {
        _inputHandler = PlayerInputHandler.Instance;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
       

        Vector3 inputDirection = new Vector3(_inputHandler.MoveInput.x, 0f, _inputHandler.MoveInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        worldDirection.Normalize();

        float speed = _walkSpeed * (_inputHandler.SprintValue > 0 ? _sprintMultiplier : 1f);
        _currentMovement.x = worldDirection.x * speed;
        _currentMovement.z = worldDirection.z * speed;

        HandleJumping();
        _characterController.Move(_currentMovement * Time.deltaTime);

    }

    private void HandleJumping()
    {
        if (_characterController.isGrounded)
        {
            _currentMovement.y = -0.5f;

            if (_inputHandler.JumpTriggered)
            {
                Debug.Log("Im Jumping!!!");

                _currentMovement.y = _jumpForce;
            }
        }
        else
        {
            _currentMovement.y -= _gravity * Time.deltaTime;
        }
    }
    private void HandleRotation()
    {
        float mouseXRotation = _inputHandler.LookInput.x * _mouseSensitivitiy;
        //_mainCamera.transform.Rotate(0f, mouseXRotation, 0f);

        verticalRotation -= _inputHandler.LookInput.y * mouseXRotation;
        verticalRotation = Mathf.Clamp(verticalRotation, -_upDownRange, _upDownRange);
        _mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }











}
