using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCameraHandler : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraFollowTarget;
    [SerializeField]
    private float _smoothCameraSpeed = 0.2f;

    private float _followDist;
    private Vector3 _cameraToBePosition;
    
    // Input Smooth Damp Variables
    private Vector3 _currentInputVector;
    private Vector3 _smoothCameraVelocity;


    private void Awake()
    {
        if (_cameraFollowTarget != null)
        {
            _followDist = Vector3.Distance(transform.position, _cameraFollowTarget.position);
            _cameraToBePosition = (transform.position - _cameraFollowTarget.position).normalized;
        }
    }

    private void Update()
    {
        if (_cameraFollowTarget != null) 
        {
            //transform.position = Vector3.SmoothDamp(transform.position, (_cameraFollowTarget.position + _followDist * _cameraToBePosition), ref _smoothCameraVelocity, _smoothCameraSpeed);
            transform.position = _cameraFollowTarget.position + _followDist * _cameraToBePosition;
        }
    }
}
