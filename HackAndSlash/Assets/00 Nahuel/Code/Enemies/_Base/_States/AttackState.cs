using System;
using UnityHFSM;

public class AttackState : EnemyStateBase
{
    public AttackState(bool needsExitTime, Enemy enemy, Action<State<Enums.EnemyStates, Enums.StateEvent>> onEnter) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        base.OnEnter();
        _animator.Play("Attack");
    }
}
