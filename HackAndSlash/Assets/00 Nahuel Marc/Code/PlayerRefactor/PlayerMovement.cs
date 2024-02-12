using MoreMountains.FeedbacksForThirdParty;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager _player;
    private Vector3 _moveDirection;

    [Header("Movement stats:")]
    public float rotationSpeed = 15f;
    public float runSpeed;
    public float maxSpeed;
    public float jumpForce;
    public float dashSpeed;
    public float dashDelay; //delay for reactive movement;

    [Header("Falling stats:")]
    public float inAirTime;
    public float leapingVelocity;
    public float fallingSpeed;
    public float raycastHeightOffset = 0.5f;
    public LayerMask groundLayer;
    public float maxDistance;

    [Header("Jump stats:")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    [Header("Movement flags:")]
    public bool isGrounded;
    private bool _isSprinting;
    private bool _canMove;
    private bool _isJumping = false;

    public bool IsSprinting
    {
        get => _isSprinting;
        set => _isSprinting = value;
    }

    private void Awake()
    {
        _player = GetComponent<PlayerManager>();
        runSpeed = maxSpeed * 0.5f;
    }

    void Start()
    {
        EnableMovement();   
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (_player.isInteracting) { return; }
        if (_isJumping) { return; }

        if (_isSprinting)
        {
            HandleMovement(maxSpeed);
        }
        else
        {
            HandleMovement(runSpeed);
        }
        HandleRotation();
    }
    public void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        Vector3 targetPosition;
        raycastOrigin.y = raycastOrigin.y + raycastHeightOffset;
        targetPosition = transform.position;

        if (Physics.Raycast(raycastOrigin, -Vector3.up, out hit, maxDistance, groundLayer))
        {
            if (!isGrounded)
            {
                _player.animations.PlayTargetAnimation(Constants.ANIMATION_LAND, true);
                _isJumping = false;
            }
            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTime = 0;
            isGrounded = true;
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

            isGrounded = false;
        }
    }
    public void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = GetDirectionNormalized();
        targetDirection.y = 0;
        if(targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = playerRotation;
        }
    }

    public void HandleMovement(float speed)
    {
        _moveDirection = GetDirectionNormalized();
        _moveDirection.y = 0;
        _moveDirection *= speed;
        _player.rb.velocity = _moveDirection;
    }

    public void EnableMovement()
    {
        _canMove = true;
        _player.ChangeCharacterState(Enums.CharacterState.Moving);
    }

    public void DisableMovement()
    {
        _canMove = false;
        _player.ChangeCharacterState(Enums.CharacterState.Idle);
        _moveDirection = Vector3.zero;
        _player.rb.velocity = Vector3.zero;
        _player.rb.angularVelocity = Vector3.zero;
    }

    public void HandleJumping()
    {
        //_player.movement.DisableMovement();
        //_player.rb.AddForce((GetDirectionNormalized() + Vector3.up) * jumpForce, ForceMode.Impulse);
        _isJumping = true;
        float jumpVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
        Vector3 playerVelocity = _moveDirection;
        playerVelocity.y = jumpVelocity;
        _player.rb.velocity = playerVelocity;
    }

    public void Dash()
    {
        if(!_canMove || _player.isInteracting)
        {
            return;
        }
        Vector2 dashDir = _player.inputs.GetDirectionLeftStick();
        if(dashDir.magnitude > 0.1f)
        {
            Vector3 dashVector = new Vector3(dashDir.x, 0, dashDir.y).normalized;

            Vector3 dashDestination = transform.position + dashVector;

            Vector3 dashDirectionNormalized = dashDestination - transform.position;
            dashDirectionNormalized.y = 0f;
            dashDirectionNormalized.Normalize();

            _player.rb.velocity = dashDirectionNormalized * dashSpeed;

            DisableMovement();
            StartCoroutine(EnableMovementAfterDash(dashDelay));
        }
        else
        {

        }
    }

    IEnumerator EnableMovementAfterDash(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnableMovement();
    }

    private Vector3 GetDirectionNormalized()
    {
        return UtilsNagu.GetCameraForward(_player.MainCamera) * _player.inputs.GetDirectionLeftStick().y + UtilsNagu.GetCameraRight(_player.MainCamera) * _player.inputs.GetDirectionLeftStick().x;
    }
    
}
