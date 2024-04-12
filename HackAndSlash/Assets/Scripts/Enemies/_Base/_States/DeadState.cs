using UnityEngine;
public class DeadState : EnemyStateBase
{
    public DeadState(bool needsExitTime, EnemyBase enemyBase) : base(needsExitTime, enemyBase) { }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        base.OnEnter();
        
        _animator.Play("Dead State");
        
        WaitExtensioNonMonobehavior.Wait(_animator.GetCurrentAnimatorClipInfo(0).Length + 1f, () =>
        {
            EnemyBase.gameObject.SetActive(false);
        });
    }
}
