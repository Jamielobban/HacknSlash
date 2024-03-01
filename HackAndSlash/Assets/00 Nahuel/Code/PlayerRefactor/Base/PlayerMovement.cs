using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager _player;
    private Vector3 _moveDirection;
    public Enums.PlayerMovementState moveState;
    public Vector3 MoveDirection
    {
        get => _moveDirection;
        set => _moveDirection = value;
    }
    #region Stats

    [Header("Speed stats:")]
    private float moveSpeed;
    public float airSpeed;
    public float runSpeed;
    public float sprintSpeed;

    // -- Dash Stats -- //
    public float dashSpeed;
    public float dashSpeedChangeFactor;
    private float speedChangeFactor;


    [Header("Rotation stats:")]
    public float rotationSpeed = 15f;
    public float airRotationSpeed = 15f;
    public float interactingRotationSpeed = 10f; // Attacking rot speed

    // public float speedIncreaseMultiplier;
    // public float slopeIncreaseMultiplier;
    // -- Momentum Stats -- //
    private float desiredVelocity;
    private float lastDesiredVelocity;
    private bool keepMomentum;
    private Enums.PlayerMovementState lastState;

    [Header("Falling stats:")]
    public float inAirTime;
    public float maxDistance;
    public LayerMask groundLayer;
    private RaycastHit _hit;
    private Vector3 _targetPosition;

    [Header("Jump stats:")]
     public float jumpHeight = 3;

    [Header("Slope stats:")]
    public float maxSlopeAngle = 60f;
    private RaycastHit _slopeHit;

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
                if (_player.rb.velocity.magnitude > dashSpeed)
                {
                    _player.rb.velocity = _player.rb.velocity.normalized * moveSpeed;
                }
            }
        }

        if (_player.isInteracting || isJumping || isDashing) 
        { 
            return; 
        }
        HandleMovement();
    }

    public float testAirTIme = .5f;
    public float testMaxDist = 3;
    public float testMinDist = .7f;

    public void HandleFallingAndLanding()
    {
        if(_player.groundCheck.isGrounded)
        {
            inAirTime = 0;
        }
        else
        {
            inAirTime += Time.deltaTime;

            if (inAirTime >= testAirTIme)
            {
                maxDistance = testMaxDist;
            }
            else
            {
                maxDistance = testMinDist;
            }

            if(isDashing)
            {
                _player.dash.ResetDash();
            }

            //If it falls not from jumping set anim fall
            if (!_player.isInteracting)
            {
                _player.animations.PlayTargetAnimation(Constants.ANIMATION_FALL, true);
            }
            if(_player.isAirAttacking)
            {
                _player.rb.velocity = Vector3.zero;
            }
        }

        _targetPosition = transform.position;

        if (Physics.Raycast(_player.groundCheck.transform.position, -Vector3.up, out _hit, maxDistance, groundLayer))
        {
            _targetPosition.y = _hit.point.y;
            if(!_player.groundCheck.isGrounded)
            {
                _player.animations.Animator.SetBool("isGrounded", true);
            }
        }
        // Anchors the player to the ground if the raycast is hiting so won't bounce in slopes, etc.
        if (_player.groundCheck.isGrounded && !isJumping)
        {
            if(inAirTime == 0f || isDashing)
            {
                if (_player.isInteracting || _player.inputs.GetDirectionLeftStick().magnitude > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = _targetPosition;
                }
            }
        }

        _player.rb.useGravity = !OnSlope() && !_player.isAirAttacking && !isDashing;
    }

    public void HandleRotation(float rotSpeed)
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = GetDirectionNormalized();
        targetDirection.y = 0;
        if (targetDirection != Vector3.zero)
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

    public Vector3 GetDirectionNormalized() => UtilsNagu.GetCameraForward(_player.MainCamera) * _player.inputs.GetDirectionLeftStick().y + UtilsNagu.GetCameraRight(_player.MainCamera) * _player.inputs.GetDirectionLeftStick().x;
    public Vector3 GetSlopeMoveDirection(Vector3 _direction) => Vector3.ProjectOnPlane(_direction, _slopeHit.normal).normalized;
    public Vector3 GetForwardSlopeDirection() => Vector3.ProjectOnPlane(transform.forward, _slopeHit.normal).normalized;

    public float GetAngleSlopeNormalForDash() => Vector3.Angle(Vector3.up, _slopeHit.normal);
    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out _slopeHit, maxDistance, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float t = 0;
        float difference = Mathf.Abs(desiredVelocity - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (t < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredVelocity, t / difference);
            t += Time.deltaTime * boostFactor;
            //if (OnSlope())
            //{
            //    float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //    float slopeAngleIncrease = 1 + (slopeAngle / 90f);

            //    t += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            //}
            //else
            //    t += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }
        //while (t < difference)
        //{
        //    moveSpeed = Mathf.Lerp(startValue, desiredVelocity, t / difference);

        //    if (OnSlope())
        //    {
        //        float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
        //        float slopeAngleIncrease = 1 + (slopeAngle / 90f);

        //        t += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
        //    }
        //    else
        //        t += Time.deltaTime * speedIncreaseMultiplier;

        //    yield return null;
        //}

        moveSpeed = desiredVelocity;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void StateHandler()
    {
        if(isDashing)
        {
            moveState = Enums.PlayerMovementState.Dashing;
            desiredVelocity = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }
        else if(_player.groundCheck.isGrounded && _isSprinting)
        {
            moveState = Enums.PlayerMovementState.Sprinting;
            desiredVelocity = sprintSpeed;
        }
        else if(_player.groundCheck.isGrounded)
        {
            moveState = Enums.PlayerMovementState.Walking;
            desiredVelocity = runSpeed * GetDirectionNormalized().magnitude;
        }
        else
        {
            moveState = Enums.PlayerMovementState.Air;
            desiredVelocity = airSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredVelocity != lastDesiredVelocity;

        if(lastState == Enums.PlayerMovementState.Dashing)
        {
            keepMomentum = true;
        }

        if(desiredMoveSpeedHasChanged)
        {
            if(keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredVelocity;
            }
        }
        lastDesiredVelocity = desiredVelocity;
        lastState = moveState;
    }
}
