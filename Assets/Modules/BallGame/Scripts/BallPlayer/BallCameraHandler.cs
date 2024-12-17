using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCameraHandler : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraFollowTarget;

    private Vector3 _cameraStartPosition;

    private void Awake()
    {
        _cameraStartPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (_cameraFollowTarget != null) 
        {
            transform.position = new Vector3(_cameraFollowTarget.position.x - _cameraStartPosition.x, _cameraFollowTarget.position.y - _cameraStartPosition.y, _cameraFollowTarget.position.z + _cameraStartPosition.z);
        }
    }
}
