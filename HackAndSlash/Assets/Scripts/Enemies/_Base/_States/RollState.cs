using System;
using UnityEngine;
using UnityHFSM;

public class RollState : EnemyStateBase
{
    public RollState(bool needsExitTime, Enemy enemy, Action<State<Enums.EnemyStates, Enums.StateEvent>> onEnter, float exitTime) : base (needsExitTime, enemy, exitTime, onEnter) {}
    private float _timer;
    public override void OnEnter()
    {
        _agent.isStopped = true;
        base.OnEnter();
    }

    public override void OnLogic()
    {
        Vector3 forwardDirection = _agent.transform.forward.normalized;
        _agent.Move( 15f * _agent.speed * forwardDirection * Time.deltaTime);
        base.OnLogic();
        _timer += Time.deltaTime;
        
        if (_timer >= _exitTime)
        {
            fsm.StateCanExit();
        }
    }
}
