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
        _player = GetComponent<PlayerManager>();
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
        float num = Random.Range(0, maxIdleAnim);
        _anim.SetFloat(Constants.ANIM_VAR_IDLE, num, idleDampTime, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false)
    {
        _anim.SetBool("isInteracting", isInteracting);
        _anim.SetBool("isUsingRootMotion", useRootMotion);
        _anim.CrossFade(targetAnimation, 0.2f);

    }

    public void OnIdleAnimation() => _anim.SetFloat(Constants.ANIM_VAR_SPEED, 0f);        
    public void OnLandingAnimation() => _player.animations.PlayTargetAnimation(Constants.ANIMATION_LAND, true);                
    public void OnFallingAnimation() => _player.animations.PlayTargetAnimation(Constants.ANIMATION_FALL, true);
    private void OnAnimatorMove()
    {
        if(_player.isUsingRootMotion)
        {
            //_player.rb.drag = 0;
            //Vector3 deltaPosition = _anim.deltaPosition;
            //Vector3 velocity = deltaPosition / Time.deltaTime;
            //_player.rb.velocity = velocity;

            // what is the animation telling us to do?
            Vector3 rootMotion = _anim.deltaPosition;

            // preserve current velocity for later
            Vector3 currentVelocity = _player.rb.velocity;

            // derive new velocity
            Vector3 animatorVelocity = rootMotion / Time.deltaTime;

            // preserve effect of gravity
          //  animatorVelocity.y = currentVelocity.y;

            // apply our derived velocity
            _player.rb.velocity = animatorVelocity;

            // just apply angular velocity as is
            Vector3 animatorAngularVelocity = _anim.angularVelocity;
            _player.rb.angularVelocity = animatorAngularVelocity;

        }
    }

}
