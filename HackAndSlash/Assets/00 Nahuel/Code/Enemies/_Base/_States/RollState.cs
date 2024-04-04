using System;
using UnityEngine;
using UnityHFSM;

public class RollState : EnemyStateBase
{
    public RollState(bool needsExitTime, Enemy enemy, Action<State<Enums.EnemyStates, Enums.StateEvent>> onEnter, float exitTime = 3f) : base (needsExitTime, enemy, exitTime, onEnter) {}

    public override void OnEnter()
    {
        _agent.isStopped = true;
        base.OnEnter();
    }

    public override void OnLogic()
    {
        _agent.Move(1.5f * _agent.speed * Time.deltaTime * _agent.transform.forward);
        base.OnLogic();
    }
}
