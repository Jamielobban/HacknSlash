using UnityEngine;
public class HitState : EnemyStateBase
{
    public HitState(bool needsExitTime, EnemyBase enemyBase) : base(needsExitTime, enemyBase) { }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        EnemyBase.attackInterrumpted = true;
        base.OnEnter();
        _animator.CrossFade("Hit State", 0.2f);
        EnemyBase.IsHit = true;
        WaitExtensioNonMonobehavior.Wait(_animator.GetCurrentAnimatorClipInfo(0).Length, () =>
        {
            Debug.Log("end");
            EnemyBase.IsHit = false;
            fsm.StateCanExit();
        });
    }
    
}
