using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : EnemyStateBase
{

    public ChaseState(bool needsExitTime, Enemy enemy, Transform target) : base(needsExitTime, enemy)
    {
        _enemy.target = target;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _agent.enabled = true;
        _agent.isStopped = false;
        _animator.CrossFade("Chase State", 0.2f);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!_requestedExit)
        {
            //More complex movement can be added Overriding Chase State
            _agent.SetDestination(_enemy.target.position);
        }
        else if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            //If we request exit, we will continue move to last known pos prior to idle
            fsm.StateCanExit();
        }
    }
}
