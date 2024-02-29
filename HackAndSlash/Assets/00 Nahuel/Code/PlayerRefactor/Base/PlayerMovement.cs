using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager _player;
    private Vector3 _moveDirection;

    #region Stats

    [Header("Movement stats:")]
    public float rotationSpeed = 15f;
    public float airRotationSpeed = 15f;
    public float interactingRotationSpeed = 10f; // Attacking rot speed

    private float moveSpeed;
    public float airSpeed;
    public float runSpeed;
    public float sprintSpeed;
    public float dashSpeed; //??

    private float desiredVelocity;
    private float lastDesiredVelocity;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    [Header("Falling stats:")]
    public float inAirTime;
    public float maxDistance;
    public LayerMask groundLayer;

    [Header("Jump stats:")]
     public float jumpHeight = 3;

    [Header("Slope stats:")]
    public float maxSlopeAngle = 60f;
    private RaycastHit slopeHit;
    #endregion

    [Header("Movement flags:")]
    private bool _isSprinting;
    public bool isJumping = false;
    public bool isDashing = false;
    public bool IsSprinting
    {
        get => _isSprinting;
        set => _isSprinting = value;
    }

    [Header("Dash stats:")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;
    public float dashDelay; //delay for reactive movement;

    private void Awake()
    {
        _player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        StateHandler();

        if (!_player.groundCheck.isGrounded)
        {
            HandleAirMovement();
        }

        //Handles rotation while attacking
        HandleRotation(interactingRotationSpeed);

        if(isDashing)
        {
            if (OnSlope())
            {
                // Apply force down if on slope 
                if (_player.rb.velocity.y > 0)
                {
                    _player.rb.AddForce(Vector3.down * 100f, ForceMode.Force);
                }
            }
        }

        if (_player.isInteracting || isJumping || isDashing) 
        { 
            return; 
        }
        HandleMovement();
    }
    public void HandleFallingAndLanding()
    {
        if(_player.groundCheck.isGrounded)
        {
            inAirTime = 0;
        }
        else
        {
            //If it falls not from jumping set anim fall
            if (!_player.isInteracting)
            {
                _player.animations.PlayTargetAnimation(Constants.ANIMATION_FALL, true);
            }

            if (_player.isInteracting && !_player.isAirAttacking)
            {
                _player.rb.useGravity = true;
                inAirTime += Time.deltaTime;
            }
            else if (_player.isAirAttacking)
            {
                _player.rb.useGravity = false;
                _player.rb.velocity = Vector3.zero;
            }
        }
        RaycastHit hit;
        Vector3 targetPosition = transform.position;
        if (Physics.Raycast(_player.groundCheck.transform.position, -Vector3.up, out hit, maxDistance, groundLayer))
        {
            targetPosition.y = hit.point.y;
            if(!_player.groundCheck.isGrounded)
            {
                _player.animations.Animator.SetBool("isGrounded", true);
            }
        }
        // Anchors the player to the ground if the raycast is hiting so won't bounce in slopes, etc.
        if (_player.groundCheck.isGrounded && !isJumping)
        {
            if (_player.isInteracting || _player.inputs.GetDirectionLeftStick().magnitude > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleRotation(float rotSpeed)
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = GetDirectionNormalized();
        targetDirection.y = 0;
        if(targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
            transform.rotation = playerRotation;
        }
    }

    public void HandleMovement()
    {
        _moveDirection = GetDirectionNormalized();

        if (OnSlope())
        {
            _player.rb.AddForce(GetSlopeMoveDirection(_moveDirection) * moveSpeed * 10f, ForceMode.Force);
            if (_player.rb.velocity.y > 0)
            {
                _player.rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
            if(_player.rb.velocity.magnitude > moveSpeed)
            {
                _player.rb.velocity = _player.rb.velocity.normalized * moveSpeed;
            }
        }

        if(_player.groundCheck.isGrounded)
        {
            _player.rb.AddForce(_moveDirection * moveSpeed * 10f, ForceMode.Force);
            //Handles normal rotation while moving
            HandleRotation(rotationSpeed);
        }
        _player.rb.useGravity = !OnSlope();
    }

    private void HandleAirMovement()
    {
        _player.rb.AddForce(GetDirectionNormalized() * moveSpeed * 10f, ForceMode.Force);
        HandleRotation(airRotationSpeed);

        //Velocity flat
        Vector3 velocity = new Vector3(_player.rb.velocity.x, 0f, _player.rb.velocity.z);

        if (velocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = velocity.normalized * moveSpeed;
            _player.rb.velocity = new Vector3(limitedVelocity.x, _player.rb.velocity.y, limitedVelocity.z);
        }
    }

    public void DisableMovement()
    {
        _player.ChangeCharacterState(Enums.CharacterState.Idle);
        _moveDirection = Vector3.zero;
        _player.rb.velocity = Vector3.zero;
        _player.rb.angularVelocity = Vector3.zero;
    }

    public void HandleJumping()
    {
        if(!_player.groundCheck.isGrounded)
        {
            return;
        }
        _player.animations.Animator.SetBool("isJumping", true);
        _player.animations.PlayTargetAnimation(Constants.ANIMATION_JUMP, true);
        JumpAction(jumpHeight);
    }

    public void JumpAction(float _jumpForce = 20)
    {
        _player.rb.velocity = new Vector3(_player.rb.velocity.x, 0, _player.rb.velocity.z);
        _player.rb.AddForce(Vector3.up * _jumpForce * 10f, ForceMode.Impulse);
    }

    public void HandleDash()
    {
        if(_player.isInteracting)
        {
            return;
        }
        if(GetDirectionNormalized().magnitude > 0.1f)
        {
            _player.animations.PlayTargetAnimation("roll", true);
            DashAction(dashForce);
        }
    }
    private void DashAction(float _dashSpeed)
    {
        isDashing = true;
        _player.isInvulnerable = true;
        DisableMovement();

        if (OnSlope())
        {
            _player.rb.AddForce(GetForwardSlopeDirection() * _dashSpeed, ForceMode.Impulse);
        }
        else
        {
            _player.rb.AddForce(transform.forward * _dashSpeed, ForceMode.Impulse);
        }
    }

    public void ResetDash()
    {
        _player.isInvulnerable = false;
        isDashing = false;
    }

    private Vector3 GetDirectionNormalized() => UtilsNagu.GetCameraForward(_player.MainCamera) * _player.inputs.GetDirectionLeftStick().y + UtilsNagu.GetCameraRight(_player.MainCamera) * _player.inputs.GetDirectionLeftStick().x;
    private Vector3 GetSlopeMoveDirection(Vector3 _direction) => Vector3.ProjectOnPlane(_direction, slopeHit.normal).normalized;
    private Vector3 GetForwardSlopeDirection() => Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out slopeHit, maxDistance, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float t = 0;
        float difference = Mathf.Abs(desiredVelocity - moveSpeed);
        float startValue = moveSpeed;

        while (t < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredVelocity, t / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                t += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                t += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredVelocity;
    }

    private void StateHandler()
    {
        if(_player.groundCheck.isGrounded && _isSprinting)
        {
            desiredVelocity = sprintSpeed;
        }
        else if(_player.groundCheck.isGrounded)
        {
            desiredVelocity = runSpeed * GetDirectionNormalized().magnitude;
        }
        else
        {
            desiredVelocity = airSpeed;
        }

        if(Mathf.Abs(desiredVelocity - lastDesiredVelocity) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredVelocity;
        }
        lastDesiredVelocity = desiredVelocity;

    }
}
