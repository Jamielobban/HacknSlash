using UnityEngine;
public class HitState : EnemyStateBase
{
    public HitState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        base.OnEnter();
        _animator.Play("Hit State");
        _enemy.IsHit = true;
        WaitExtensioNonMonobehavior.Wait(_animator.GetCurrentAnimatorClipInfo(0).Length, () =>
        {
            Debug.Log("end");
            _enemy.IsHit = false;
            fsm.StateCanExit();
        });
    }
    
}
