using System;
using UnityEngine;
using UnityHFSM;

public class AttackState : EnemyStateBase
{
    public AttackState(bool needsExitTime, EnemyBase enemyBase, Action<State<Enums.EnemyStates, Enums.StateEvent>> onEnter, float exitTime = 3f) : base(needsExitTime, enemyBase, exitTime, onEnter) { }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        base.OnEnter();
    }

    public override void OnLogic()
    {
        Debug.Log("in Attacking");
        if (!_enemyBase.isAttacking)
        {
            Debug.Log("End Attacking");
            fsm.StateCanExit();
        }
    }
}
