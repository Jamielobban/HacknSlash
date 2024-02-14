using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _anim;
    private PlayerManager _player;

    public float idleDampTime = 0.1f;
    public float timeChangeIdle = 5f;
    public int maxIdleAnim;
    private float _timer = 0f;

    public Animator GetAnimator
    {
        get { return _anim; }
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = transform.parent.GetComponent<PlayerManager>();
    }


    public void ConcatenateAttack()
    {
        _player.comboController.ConncatenateAttack();
    }

    public void HandleMovingAnimations()
    {
        if(_player.movement.IsSprinting)
        {
            _anim.SetFloat(Constants.ANIM_VAR_SPEED, 2f);
        }
        else if(_player.inputs.GetDirectionLeftStick().magnitude > 0.125f)
        {
            _anim.SetFloat(Constants.ANIM_VAR_SPEED, _player.inputs.GetDirectionLeftStick().magnitude);
        }
    }

    public void HandleIdleAnimations()
    {
        _timer += Time.deltaTime;
        if(_timer >= timeChangeIdle)
        {
            SetRandomIdleAnimation();
            _timer = 0;
        }
    }

    private void SetRandomIdleAnimation()
    {
        int num = Random.Range(0, maxIdleAnim);
        _anim.SetFloat(Constants.ANIM_VAR_IDLE, num, idleDampTime, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        _anim.SetBool("isInteracting", isInteracting);
        _anim.CrossFade(targetAnimation, 0.2f);
    }

    public void OnIdleAnimation() => _anim.SetFloat(Constants.ANIM_VAR_SPEED, 0f);        
    public void OnLandingAnimation() => _player.animations.PlayTargetAnimation(Constants.ANIMATION_LAND, true);                
    public void OnFallingAnimation() => _player.animations.PlayTargetAnimation(Constants.ANIMATION_FALL, true);
}
