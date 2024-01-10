using System.Collections;
using UnityEngine;

public class EnemyBaseAttack : MonoBehaviour
{
    public EnemyAttackData data;
    public Enums.EnemyAttackState currentAttackState = Enums.EnemyAttackState.ReadyToUse;
    protected float _currentTime;
    public Enemy enemy;
    private bool _animationOver;
    protected virtual void Awake()
    {
        _animationOver = false;
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if(currentAttackState == Enums.EnemyAttackState.Cooldown)
        {
            _currentTime -= Time.deltaTime;
            if(!IsInCd())
            {
                ResetAbility();
            }
        }
    }

    public virtual void Use()
    {
        if(IsReadToUse())
        {
            StartCoroutine(HandleAttack());
        }
    }

    private IEnumerator HandleAttack()
    {
        currentAttackState = Enums.EnemyAttackState.Casting;
        PlayAttackAnimation();
        yield return new WaitForSeconds(data.castTime.Value);
        SetVisualEffects();
        AttackAction();
        _currentTime = data.cooldown.Value;
        currentAttackState = Enums.EnemyAttackState.Cooldown;
    }

    protected virtual void PlayAttackAnimation()
    {
        _animationOver = false;
        enemy.animations.PlayTargetAnimation("" + data.animation, true);
        this.Wait(enemy.animations.Animator.GetCurrentAnimatorClipInfo(0).Length, () =>
        {
            _animationOver = true;
        });
    }
    protected virtual void SetVisualEffects()
    {

    }
    protected virtual void AttackAction()
    {

    }
    public virtual bool IsInRangeToAttack(float dist)
    {
        return dist <= data.range.Value;
    }

    public virtual bool IsReadToUse() => currentAttackState == Enums.EnemyAttackState.ReadyToUse;
    public virtual bool IsInCd() => _currentTime < data.cooldown.Value && _currentTime >= 0;
    public virtual void ResetAbility() => currentAttackState = Enums.EnemyAttackState.ReadyToUse;
    public virtual bool IsAtkAnimaitonOver() => _animationOver;
}
