using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerManager _player;
    private Animator _anim;
    public Animator Animator => _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<PlayerManager>();
    }
    private float _timer = 0.0f;
    public float timeToDeactive = .25f;
    public void HandleMovingAnimations()
    {
        if (_player.inputs.GetDirectionLeftStick().magnitude < 0f)
        {
            _timer += Time.deltaTime;
            if (_timer > timeToDeactive && _player.groundCheck.isGrounded)
            {
                Debug.Log("canceled");
                _player.movement.DisableMovement();
                _player.ChangeCharacterState(Enums.CharacterState.Idle);
                _timer = 0f;
            }
        }
        if(_player.inputs.GetDirectionLeftStick().magnitude > 0 && _player.movement.moveSpeed > 0)
        {
            if (_player.movement.IsSprinting)
            {
                _anim.SetFloat(Constants.ANIM_VAR_SPEED, 2f);
            }
            else
            {
                _anim.SetFloat(Constants.ANIM_VAR_SPEED, _player.inputs.GetDirectionLeftStick().magnitude);
            }
        }
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        _anim.SetBool("isInteracting", isInteracting);
        _anim.CrossFade(targetAnimation, 0.2f);
    }

    public void OnLandingAnimation() => _player.animations.PlayTargetAnimation(Constants.ANIMATION_LAND, true);                
    public void OnFallingAnimation() => _player.animations.PlayTargetAnimation(Constants.ANIMATION_FALL, true);

    public void OnIdleAnimation()
    {
        if (!_player.movement.IsSprinting)
        {
            StopAllCoroutines();
            StartCoroutine(DecreaseSpeedOverTime(0f, .45f));
        }
        else
        {
            StopAllCoroutines();
            _anim.SetFloat(Constants.ANIM_VAR_SPEED, 1f);
            StartCoroutine(DecreaseSpeedOverTime(0f, .15f));
        }
    }

    public IEnumerator DecreaseSpeedOverTime(float targetValue, float _duration)
    {
        float currentSpeedAnimation = _anim.GetFloat(Constants.ANIM_VAR_SPEED);
        float currentSpeed = _player.movement.moveSpeed;
        float elapsedTime = 0f;
        float duration = _duration; 

        while (elapsedTime < duration)
        {
            float newSpeed = Mathf.Lerp(currentSpeed, targetValue, elapsedTime / duration);
            float newAnimationSpeed = Mathf.Lerp(currentSpeedAnimation, targetValue, elapsedTime / duration);
            _player.movement.moveSpeed = newSpeed;
            _anim.SetFloat(Constants.ANIM_VAR_SPEED, newAnimationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _anim.SetFloat(Constants.ANIM_VAR_SPEED, 0f);
        _player.movement.moveSpeed = 0f;
    }

    private IEnumerator IncreaseOverTime(float currentSpeed, float targetValue)
    {
        float elapsedTime = 0f;
        float duration = 0.15f;
        while (elapsedTime < duration)
        {
            float newSpeedAnimation = Mathf.Lerp(currentSpeed, targetValue, elapsedTime / duration);
            _anim.SetFloat(Constants.ANIM_VAR_SPEED, newSpeedAnimation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _anim.SetFloat(Constants.ANIM_VAR_SPEED, targetValue);
    }

}
