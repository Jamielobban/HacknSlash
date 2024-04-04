using UnityEngine;
public class IdleState : EnemyStateBase
{
    public IdleState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _animator.Play("Idle");
    }
}
