using UnityEngine;
public class DeadState : EnemyStateBase
{
    public DeadState(bool needsExitTime, EnemyBase enemyBase) : base(needsExitTime, enemyBase) { }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        base.OnEnter();
        
        _animator.CrossFadeInFixedTime("Dead State", 0.2f);
        
        WaitExtensioNonMonobehavior.Wait(_animator.GetCurrentAnimatorClipInfo(0).Length + 1f, () =>
        {
            _enemyBase.gameObject.SetActive(false);
        });
    }
}
