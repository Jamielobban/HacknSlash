using UnityEngine;
using UnityHFSM;
using System;
public class AreaState : EnemyStateBase
{
    public AreaState(bool needsExitTime, EnemyBase enemyBase, Action<State<Enums.EnemyStates, Enums.StateEvent>> onEnter, float exitTime = 3f) : base(needsExitTime, enemyBase, exitTime, onEnter) { }
    public override void OnEnter()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        base.OnEnter();
    }
}
