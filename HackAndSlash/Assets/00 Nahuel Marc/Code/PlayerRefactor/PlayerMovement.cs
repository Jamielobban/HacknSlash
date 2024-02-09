using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{

    private PlayerManager _player;
    [Header("Falling stats:")]
    public float inAirTime;
    public float leapingVelocity;
    public float fallingSpeed;
    public float raycastHeightOffset = 0.5f;
    public LayerMask groundLayer;
    public float maxDistance;

    [Header("Movement flags:")]
    public bool isGrounded;
    public bool isSprinting;

    private void Awake()
    {
        _player = GetComponent<PlayerManager>();
    }

    void Start()
    {
        EnableMovement();   
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HandleAllMovement()
    {

    }

    public void HandleFallingAndLanding()
    {
        /*
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        Vector3 targetPosition;
        raycastOrigin.y = raycastOrigin.y + raycastHeightOffset;
        targetPosition = transform.position;

        if (Physics.Raycast(raycastOrigin, -Vector3.up, out hit, maxDistance, groundLayer))
        {
            if (!isGrounded && !_player.isInteracting)
            {
                _player.animations.PlayTargetAnimation(Constants.ANIMATION_LAND, true);
            }
            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTime = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded)
        {
            transform.position = targetPosition;
            if (_player.isInteracting || _moveDirection != Vector3.zero)
            {
                //   transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
            }
            else
            {
                // transform.position = targetPosition;
            }
        }
        else
        {
            if (!_player.isInteracting)
            {
                _player.animations.PlayTargetAnimation(Constants.ANIMATION_FALL, true);
            }
            inAirTime += Time.deltaTime;
            _player.rb.AddForce(transform.forward * leapingVelocity);
            _player.rb.AddForce(-Vector3.up * fallingSpeed * inAirTime);
        }*/
    }

    public void EnableMovement()
    {

    }

    public void DisableMovement()
    {

    }
}
