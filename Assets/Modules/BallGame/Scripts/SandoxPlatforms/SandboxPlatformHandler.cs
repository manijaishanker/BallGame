using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SandboxPlatformHandler : MonoBehaviour
{
    [SerializeField]
    private float _bounceBackForce = 10f;

    // We want the force to be applied only to the player layer
    private const string BALL_PLAYER_TAG = "Player";
    Vector3 _dirOfImp = Vector3.forward;

    private void OnCollisionEnter(Collision collision)
    {
        ApplyBounceForce(collision);
    }

    private void ApplyBounceForce(Collision collision)
    {
        if (collision.gameObject.CompareTag(BALL_PLAYER_TAG))
        {
            Rigidbody playerRB = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRB == null)
            {
                Debug.LogError("No RigidBody to apply force to");
                return;
            }

            Vector3 directionOfImpact = collision.impulse.normalized;
            _dirOfImp = directionOfImpact;

            if (directionOfImpact.sqrMagnitude == 1) 
                playerRB.AddForce(directionOfImpact * _bounceBackForce, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(_dirOfImp * 1000f));

    }
}
