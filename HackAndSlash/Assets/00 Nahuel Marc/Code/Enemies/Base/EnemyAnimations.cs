using UnityEngine;

// -- Base Animations class, in Animator base animation as "Die", "Hit", "Stun" should be named equal always -- //
public class EnemyAnimations : MonoBehaviour
{
    private Animator _anim;
    public Animator Animator => _anim;
    protected EnemyEvents _events;
    protected virtual void Awake()
    {
        _events = transform.parent.GetComponent<EnemyEvents>();
        _anim = GetComponent<Animator>();

        _events.OnIdle += () => _anim.SetFloat("speed", 0);
        _events.OnPatrolling += () => _anim.SetFloat("speed", 0.5f);
        _events.OnAttacking += () => _anim.SetFloat("speed", 0);
        _events.OnFollowing += () => _anim.SetFloat("speed", 1);
        _events.OnHit += () => PlayTargetAnimation("Hit", true);
        _events.OnStun += () => PlayTargetAnimation("Stun", true);
        _events.OnDie += () => DeadAnimEnd();
    }

    public virtual void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        _anim.SetBool("isInteracting", isInteracting);
        _anim.CrossFade(targetAnimation, 0.2f);
    }
    //CHECK
    private void DeadAnimEnd()
    {
        PlayTargetAnimation("Die", true);
        this.Wait(Animator.GetCurrentAnimatorClipInfo(0).Length, () =>
        {
            //Respawn?
            Debug.Log("Hola");
        });
    }
}
