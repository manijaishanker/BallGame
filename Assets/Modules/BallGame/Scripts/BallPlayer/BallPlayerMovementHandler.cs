using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class BallPlayerMovementHandler : MonoBehaviour
{
    [SerializeField]
    InputActionAsset _ballPlayerActionMap;

    // Allowed Ball Outside Variables (Speed, Acceleration, etc.)
    [SerializeField]
    private float _ballMaxSpeed = 5f;
    [SerializeField]
    private float _ballAccelerationRate = 0.25f;
    [SerializeField]
    private float _ballJumpForce = 15f;
    [SerializeField]
    private float _smoothInputSpeed = 0.2f;
    [SerializeField]
    private float _gravity = 9.8f;

    public enum EBallPlayerState
    {
        None = 0,
        Idle = 1,
        Moving = 2,
        Jumping = 3,
        Falling = 4
    }

    public EBallPlayerState BallPlayerState { get { return _ballPlayerState; } }
    private EBallPlayerState _ballPlayerState;

    // Ball Player Movement Action Map InputActions 
    private InputAction _ballPlayerMovementAction;
    private InputAction _ballPlayerJumpAction;

    // Ball Core Properties (GetComponents on the Attached Transform)
    // These Should Not Be Null
    private Transform _ballTransform;
    private Rigidbody _ballRigidBody;

    // Ball Core Movement properties
    private Vector3 _ballMovementVelocity;
    private float _verticalVelocity; 
    private float _ballCurrentSpeed;
    private float _ballMaxJumpHeight = 1f;
    private float _ballJumpStartPos; // Variable is used to store the initial Y pos when the ball is jumped

    // Input Smooth Damp Variables
    private Vector3 _currentInputVector;
    private Vector3 _smoothInputVelocity;
    

    private void Awake()
    {
        _ballTransform = GetComponent<Transform>();
        _ballRigidBody = GetComponent<Rigidbody>();

        _ballPlayerMovementAction = _ballPlayerActionMap.FindAction("Movement");
        _ballPlayerJumpAction = _ballPlayerActionMap.FindAction("Jump");

        _ballPlayerJumpAction.started += HandleJumping;
    }

    private void Update()
    {
        ReadMovementInput();

        if (IsBallFalling())
            _ballPlayerState = EBallPlayerState.Falling;
        if (!IsBallFalling() && (_ballPlayerState != EBallPlayerState.Jumping || _ballPlayerState != EBallPlayerState.Moving)) //  && (_ballRigidBody.velocity.magnitude <= new Vector3(0.1f, 0f, 0.1f).magnitude)
        {
            _ballPlayerState = EBallPlayerState.Idle;
        }
    }

    private void FixedUpdate()
    {
        AddMovementForce();
    }

    private void ReadMovementInput()
    {
        Vector2 ballMovementInputDirection = _ballPlayerMovementAction.ReadValue<Vector2>();

        if (ballMovementInputDirection == Vector2.zero)
        {
            _ballMovementVelocity = Vector3.zero;
            _ballCurrentSpeed = 0f;
            _ballPlayerState = EBallPlayerState.Idle;
        }

        _currentInputVector = Vector3.SmoothDamp(_currentInputVector, ballMovementInputDirection, ref _smoothInputVelocity, _smoothInputSpeed);

        Vector3 ballMovementDirection = new Vector3(_currentInputVector.x, 0f, _currentInputVector.y);

        _ballCurrentSpeed += _ballAccelerationRate * Time.deltaTime;
        _ballCurrentSpeed = Mathf.Clamp(_ballCurrentSpeed, 0f, _ballMaxSpeed);

        Debug.Log("!!!!!!11 CURRENT SPEED: " + _ballCurrentSpeed);

        _ballMovementVelocity = ballMovementDirection.normalized * _ballCurrentSpeed;
        Debug.Log("!!!!!!11 CURRENT VELOCITY: " + _ballMovementVelocity);

        //float acceleration = _ballAccelerationRate * Time.deltaTime;
        _ballMovementVelocity = Vector3.ClampMagnitude(_ballMovementVelocity, _ballMaxSpeed);

        //Debug.Log("!!!!1 HANDLE JUMPING: ISGROUNDED: " + IsGrounded() + " BallPlayerState: " + BallPlayerState);

    }

    private void HandleJumping(InputAction.CallbackContext callbackContext)
    {
        //Debug.Log("!!!!1 HANDLE JUMPING: ISGROUNDED: " + IsGrounded() + " BallPlayerState: " + BallPlayerState);
        if (IsGrounded() && BallPlayerState != EBallPlayerState.Falling)
        {
            AddJumpForce();
        }
    }

    private void AddMovementForce()
    {
        //if (_ballRigidBody.velocity.sqrMagnitude < _ballMaxSpeed * _ballMaxSpeed)
        //{
        _ballRigidBody.velocity = _ballMovementVelocity;//.AddForce(_ballMovementVelocity, ForceMode.Impulse);
            _ballPlayerState = EBallPlayerState.Moving;
        //}
    }

    private void AddJumpForce()
    {
        _ballPlayerState = EBallPlayerState.Jumping;
        _verticalVelocity += Mathf.Sqrt(_ballJumpForce * 3 * _gravity);
        Vector3 ballJumpDirection = new Vector3(0f, 1f * _verticalVelocity, 0f);

        _ballRigidBody.AddForce(ballJumpDirection, ForceMode.Impulse);
        _verticalVelocity = 0f;

        _ballJumpStartPos = _ballTransform.position.y; // Store the value when the ball left the ground
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(_ballTransform.position, -Vector3.up, 1f);
    }
    private bool IsBallFalling()
    {
        return _ballTransform.position.y - _ballJumpStartPos >= _ballMaxJumpHeight;
    }

}
