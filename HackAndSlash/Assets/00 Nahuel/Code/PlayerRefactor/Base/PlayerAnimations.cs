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

    public void HandleMovingAnimations()
    {
        if(_player.movement.IsSprinting)
        {
            StartCoroutine(IncreaseOverTime(_anim.GetFloat(Constants.ANIM_VAR_SPEED),2f));
        }
        else if(_player.inputs.GetDirectionLeftStick().magnitude > 0f)
        {
            _anim.SetFloat(Constants.ANIM_VAR_SPEED, _player.inputs.GetDirectionLeftStick().magnitude);
        }
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false)
    {
        _anim.SetBool("isInteracting", isInteracting);
        _anim.SetBool("isUsingRootMotion", useRootMotion);
        _anim.CrossFade(targetAnimation, 0.2f);
    }

    public void OnLandingAnimation() => _player.animations.PlayTargetAnimation(Constants.ANIMATION_LAND, true);                
    public void OnFallingAnimation() => _player.animations.PlayTargetAnimation(Constants.ANIMATION_FALL, true);

    public void OnIdleAnimation()
    {
        StartCoroutine(DecreaseSpeedOverTime(0f));
    }

    public IEnumerator DecreaseSpeedOverTime(float targetValue)
    {
        float currentSpeed = _anim.GetFloat(Constants.ANIM_VAR_SPEED);
        float elapsedTime = 0f;
        float duration = 0.5f; 

        while (elapsedTime < duration)
        {
            float newSpeed = Mathf.Lerp(currentSpeed, targetValue, elapsedTime / duration);
            _anim.SetFloat(Constants.ANIM_VAR_SPEED, newSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _anim.SetFloat(Constants.ANIM_VAR_SPEED, targetValue);
    }

    private IEnumerator IncreaseOverTime(float currentSpeed, float targetValue)
    {
        float elapsedTIme = 0f;
        float duration = 0.5f;
        while (elapsedTIme < duration)
        {
            float newSpeed = Mathf.Lerp(currentSpeed, targetValue, elapsedTIme / duration);
            _anim.SetFloat(Constants.ANIM_VAR_SPEED, newSpeed);
            elapsedTIme += Time.deltaTime;
            yield return null;
        }
        _anim.SetFloat(Constants.ANIM_VAR_SPEED, targetValue);
    }

}
