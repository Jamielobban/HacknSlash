using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager _player;
    private Vector3 _moveDirection;

    #region Stats

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
    public const int maxJumps = 2;
    private int _currentJumps = 0;

    #endregion

    [Header("Movement flags:")]
    public bool isGrounded;
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
        runSpeed = maxSpeed * 0.5f;
    }

    private void Update()
    {

    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (_player.isInteracting || isJumping || isDashing) { 
            return; }

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
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + raycastHeightOffset;
        Vector3 targetPosition = transform.position;

       // if(!isGrounded && !isJumping)
        if(!isGrounded)
        {
            if(!_player.isInteracting)
            {
               // _player.animations.PlayTargetAnimation(Constants.ANIMATION_FALL, true);
            }
            if(_player.isInteracting && !_player.isAirAttacking)
            {
                inAirTime += Time.deltaTime;
                _player.rb.AddForce(transform.forward * leapingVelocity);
                _player.rb.AddForce(-Vector3.up * fallingSpeed * inAirTime);
            }
            else if(_player.isAirAttacking)
            {
                _player.rb.velocity = Vector3.zero;
            }
            
        }

        if(Physics.Raycast(rayCastOrigin, -Vector3.up, out hit, maxDistance, groundLayer))
        {
            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTime = 0;
            isGrounded = true;
            ResetJumps();
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping && !isDashing)
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

    public void DisableMovement()
    {
        _player.ChangeCharacterState(Enums.CharacterState.Idle);
        _moveDirection = Vector3.zero;
        _player.rb.velocity = Vector3.zero;
        _player.rb.angularVelocity = Vector3.zero;
    }

    public void HandleJumping()
    {
        if(CanJump())
        {
            _currentJumps++;
            _player.animations.Animator.SetBool("isJumping", true);
            _player.animations.PlayTargetAnimation(Constants.ANIMATION_JUMP, true);
        }
    }

    private void ResetJumps()
    {
        _currentJumps = 0;
    }

    public void JumpAction(float _jumpHeight)
    {
        float jumpVelocity = Mathf.Sqrt(-2 * gravityIntensity * _jumpHeight);
        Vector3 playerVelocity = _moveDirection;
        playerVelocity.y = jumpVelocity;
        _player.rb.velocity = playerVelocity;
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
            HandleDash();
        }
    }

    private void HandleDash()
    {
        isDashing = true;
        _player.isInvulnerable = true;
        DisableMovement();
        _player.rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
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

    private bool CanJump() => _currentJumps <= maxJumps;

}
