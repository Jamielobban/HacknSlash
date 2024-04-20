using UnityEngine;
public class IdleState : EnemyStateBase
{
    public IdleState(bool needsExitTime, EnemyBase enemyBase) : base(needsExitTime, enemyBase) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _animator.CrossFade("Idle", 0.2f);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if(_enemyBase.isAttacking)
        {
            fsm.StateCanExit();
        }
    }
}
