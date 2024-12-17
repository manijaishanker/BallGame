using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallPlayerMovementHandler : MonoBehaviour
{
    [SerializeField]
    InputActionAsset _ballPlayerActionMap;

    // Allowed Ball Outside Variables (Speed, Acceleration, etc.)
    [SerializeField]
    private float _ballMaxSpeed = 5f;
    [SerializeField]
    private float _ballJumpForce = 15f;

    // Ball Player Movement Action Map InputActions 
    private InputAction _ballPlayerMovementAction;
    private InputAction _ballPlayerJumpAction;


    // Ball Core Properties (GetComponents on the Attached Transform)
    // These Should Not Be Null
    private Transform _ballTransform;
    private Rigidbody _ballRigidBody;

    private Vector3 _ballMovementVelocity;

    public enum EBallPlayerState
    {
        None = 0,
        Idle = 1,
        Moving = 2,
        Jumping = 3
    }

    public EBallPlayerState BallPlayerState { get { return _ballPlayerState; } }
    private EBallPlayerState _ballPlayerState;

    private void Awake()
    {
        _ballTransform = GetComponent<Transform>();
        _ballRigidBody = GetComponent<Rigidbody>();

        _ballPlayerMovementAction = _ballPlayerActionMap.FindAction("Movement");
        _ballPlayerJumpAction = _ballPlayerActionMap.FindAction("Jump");

        _ballPlayerJumpAction.started += HandleJumping;
        _ballPlayerJumpAction.canceled += (value) => { _ballPlayerState = EBallPlayerState.Idle; };
        _ballPlayerMovementAction.performed += (value) => { _ballPlayerState = EBallPlayerState.Moving; };
        _ballPlayerMovementAction.canceled += (value) => { _ballPlayerState = EBallPlayerState.Idle; };

    }

    private void Update()
    {
        ReadMovementInput();
        Debug.Log("JUMP VECTOR MAGNITUDE: " + _ballTransform.up.magnitude);
    }

    private void FixedUpdate()
    {
        AddMovementForce();
    }

    private void ReadMovementInput()
    {
        Vector2 ballMovementInputProperty = _ballPlayerMovementAction.ReadValue<Vector2>();
        Vector3 ballMovementDirection = new Vector3(ballMovementInputProperty.x * _ballMaxSpeed, 1f, ballMovementInputProperty.y * _ballMaxSpeed);
        _ballMovementVelocity += ballMovementDirection * Time.deltaTime;
    }

    private void HandleJumping(InputAction.CallbackContext callbackContext)
    {
        if (_ballPlayerState != EBallPlayerState.Jumping)
        {
            _ballPlayerState = EBallPlayerState.Jumping;
            AddJumpForce();
        }
    }

    private void AddMovementForce()
    {
        _ballRigidBody.AddForce(_ballMovementVelocity, ForceMode.Impulse);
        _ballMovementVelocity = Vector3.zero;
    }

    private void AddJumpForce()
    {
        Vector3 ballMovementDirection = new Vector3(_ballMovementVelocity.x, 1 * _ballJumpForce, _ballMovementVelocity.y);
        _ballMovementVelocity += ballMovementDirection;

        _ballRigidBody.AddForce(_ballMovementVelocity, ForceMode.Impulse);
        _ballMovementVelocity = Vector3.zero;
    }
}
