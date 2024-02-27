using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager _player;
    private Vector3 _moveDirection;

    #region Stats

    [Header("Movement stats:")]
    public float rotationSpeed = 15f;
    public float airRotationSpeed = 15f;
    public float interactingRotationSpeed = 10f;

    public float airSpeed;
    public float runSpeed;
    public float maxSpeed;
    public float dashSpeed;
    public float dashDelay; //delay for reactive movement;

    [Header("Falling stats:")]
    public float inAirTime;
    public float leapingVelocity;
    public float fallingSpeed;
    public float maxGravitySpeed;
    public float raycastHeightOffset = 0.5f;
    public LayerMask groundLayer;
    public float maxDistance;

    [Header("Jump stats:")]
     public float jumpHeight = 3;
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

        if(!_player.groundCheck.isGrounded)
        {
            HandleAirMovement();
            //Handles rotation if pj is falling
            HandleRotation(rotationSpeed);
        }

        //Handles rotation while attacking
        HandleRotation(interactingRotationSpeed);

        if (_player.isInteracting || isJumping || isDashing || !_player.groundCheck.isGrounded) 
        { 
            return; 
        }

        if (_isSprinting)
        {
            HandleMovement(maxSpeed);
        }
        else
        {
            HandleMovement(runSpeed);
        }
        //Handles normal rotation while moving
        HandleRotation(rotationSpeed);
    }
    public void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + raycastHeightOffset;
        Vector3 targetPosition = transform.position;

        if(_player.groundCheck.isGrounded)
        {
            if(inAirTime >= 1.5f)
            {
                _player.animations.PlayTargetAnimation(Constants.ANIMATION_LAND, true);
            }

            inAirTime = 0;
        }
        else
        {
            if(!_player.isInteracting)
            {
                _player.animations.PlayTargetAnimation(Constants.ANIMATION_FALL, true);
            }

            if(_player.isInteracting && !_player.isAirAttacking)
            {
                _player.rb.useGravity = true;
                inAirTime += Time.deltaTime;
                _player.rb.AddForce(transform.forward * leapingVelocity);
                _player.rb.AddForce(-Vector3.up * fallingSpeed * inAirTime);
            }
            else if (_player.isAirAttacking)
            {
                _player.rb.useGravity = false;
                _player.rb.velocity = Vector3.zero;
            }
        }

        if(Physics.Raycast(rayCastOrigin, -Vector3.up, out hit, maxDistance, groundLayer))
        {
            targetPosition.y = hit.point.y;
        }

        if (_player.groundCheck.isGrounded && !isJumping && !isDashing)
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

    public void HandleMovement(float speed)
    {
        _moveDirection = GetDirectionNormalized();
        _moveDirection.y = 0;
        _moveDirection *= speed;
        _player.rb.velocity = _moveDirection;
    }

    public void HandleAirMovement()
    {
        _moveDirection = GetDirectionNormalized();
        _moveDirection *= airSpeed;
        _player.rb.velocity += _moveDirection;
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
    }

    public void JumpAction(float _jumpHeight = 20)
    {
        _player.rb.AddForce(Vector3.up * _jumpHeight , ForceMode.Impulse);
    }

    public void Dash()
    {
        if(_player.isInteracting)
        {
            return;
        }
        if(GetDirectionNormalized().magnitude > 0.1f)
        {
            _player.animations.PlayTargetAnimation("roll", true);
            HandleDash(dashSpeed);
        }
    }

    private void HandleDash(float _dashSpeed)
    {
        isDashing = true;
        _player.isInvulnerable = true;
        DisableMovement();
        _player.rb.AddForce(transform.forward * _dashSpeed, ForceMode.Impulse);
        StartCoroutine(EnableMovementAfterDash(dashDelay));
    }

    IEnumerator EnableMovementAfterDash(float delay)
    {
        yield return new WaitForSeconds(delay);
        _player.isInvulnerable = false;
        isDashing = false;
    }

    private Vector3 GetDirectionNormalized()
    {
        return UtilsNagu.GetCameraForward(_player.MainCamera) * _player.inputs.GetDirectionLeftStick().y + UtilsNagu.GetCameraRight(_player.MainCamera) * _player.inputs.GetDirectionLeftStick().x;
    }


}
